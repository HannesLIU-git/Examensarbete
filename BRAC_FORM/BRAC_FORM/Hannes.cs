using NXOpen;
using NXOpen.Assemblies;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace BRAC_FORM
{
    public class Hannes
    {
        private TextBox textBox1;
        private TextBox textBox2;

        public Hannes(TextBox tb1, TextBox tb2)
        {
            textBox1 = tb1;
            textBox2 = tb2;
        }

        public void DeleteCubes()
        {
            int numberOfParts = int.Parse(textBox1.Text);

            Session theSession = Session.GetSession();

            // ✅ Use the currently open assembly as work part

            string partsFolderPath = @"C:\Users\timpe989\source\repos\from - weapon - brac\BRAC_FORM\CAD\";

            try
            {
                // Get the assembly part
                Part assemblyPart = (Part)theSession.Parts.Work;
                if (assemblyPart == null)
                {
                    Console.WriteLine("Assembly not found.");
                    return;
                }

                ComponentAssembly componentAssembly = assemblyPart.ComponentAssembly;
                Component rootComponent = componentAssembly.RootComponent;


                for (int i = 1; i < numberOfParts + 1; i++)
                {
                    string partNameToDelete = "KUB_" + i;
                    string partFilePath = Path.Combine(partsFolderPath, partNameToDelete + ".prt");

                    Component[] components = rootComponent.GetChildren();
                    Component partToDelete = null;

                    foreach (var component in components)
                    {
                        if (component.Prototype.Name == partNameToDelete)
                        {
                            partToDelete = component;
                            break;
                        }
                    }

                    if (partToDelete != null)
                    {
                        // Remove the part from the assembly
                        componentAssembly.RemoveComponent(partToDelete);
                        Console.WriteLine($"Part '{partNameToDelete}' removed from the assembly.");

                    }
                    else
                    {
                        Console.WriteLine($"Part '{partNameToDelete}' not found in the assembly.");
                    }

                       
                            // Check if the file exists, then delete it from the file system

                    if (File.Exists(partFilePath))
                    {
                        File.Delete(partFilePath);
                        Console.WriteLine($"Part file '{partFilePath}' deleted from disk.");
                    }
                        
                    else
                    {
                        Console.WriteLine("Part not found in the assembly.");
                    }
                }
                
            }

            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        
        public void AddCubes()
        {
            AddPartsInDirection("KUB", new Point3d(110.0, 0.0, 0.0), @"C:\Users\timpe989\source\Repos\from-weapon-brac\BRAC_FORM\CAD\KUB.prt");
        }

        public void AddCylinders()
        {
            AddPartsInDirection("CYL", new Point3d(0.0, 110.0, 0.0), @"C:\Users\timpe989\source\Repos\from-weapon-brac\BRAC_FORM\CAD\CYL.prt");
        }

        private void AddPartsInDirection(string componentPrefix, Point3d directionOffset, string masterPartPath)
        {
            try
            {
                Session theSession = Session.GetSession();

                // ✅ Use the currently open assembly as work part
                Part assemblyPart = (Part)theSession.Parts.Work;

                // Check if a assembly is open
                if (assemblyPart == null || assemblyPart.ComponentAssembly == null)
                {
                    UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "No assembly is currently open as the work part.");
                    return;
                }


                // Define the number of parts to add
                int numberOfParts = int.Parse(textBox1.Text);

                // Define the wanted parameter value
                string dimValue = textBox2.Text;

                // Create a point in origo
                Point3d position = new Point3d(0.0, 0.0, 0.0);

                // For loop for insantiating
                for (int i = 0; i < numberOfParts; i++)
                {
                    // Create new filepath, Combines old filepath and prefix (Cyl or Kub) and part number
                    string newPartPath = Path.Combine(Path.GetDirectoryName(masterPartPath), $"{componentPrefix}_{i + 1}.prt");

                    // Copy the master part to create a unique part file
                    File.Copy(masterPartPath, newPartPath, true);

                    // Create new basepart
                    PartLoadStatus loadStatus;
                    BasePart copiedPart = theSession.Parts.OpenBase(newPartPath, out loadStatus);
                    loadStatus.Dispose();

                    AddComponentBuilder addComponentBuilder = assemblyPart.AssemblyManager.CreateAddComponentBuilder();

                    addComponentBuilder.SetPartsToAdd(new BasePart[] { copiedPart });
                    addComponentBuilder.ComponentName = $"{componentPrefix}_{i + 1}";
                    addComponentBuilder.ReferenceSet = "Entire Part";

                    Matrix3x3 orientation = new Matrix3x3
                    {
                        Xx = 1.0,
                        Xy = 0.0,
                        Xz = 0.0,
                        Yx = 0.0,
                        Yy = 1.0,
                        Yz = 0.0,
                        Zx = 0.0,
                        Zy = 0.0,
                        Zz = 1.0
                    };

                    addComponentBuilder.SetInitialLocationAndOrientation(position, orientation);

                    Component newComponent = addComponentBuilder.Commit() as Component;
                    addComponentBuilder.Destroy();

                    // Move position for next part
                    position.X += directionOffset.X;
                    position.Y += directionOffset.Y;
                    position.Z += directionOffset.Z;
                }

                // Set or update expression "Dim"
                Expression dimExpression;
                try
                {
                    dimExpression = assemblyPart.Expressions.FindObject("Dim");
                }
                catch
                {
                    dimExpression = assemblyPart.Expressions.CreateExpression("Number", "Dim=1.0");
                }
                dimExpression.RightHandSide = dimValue;

                theSession.Parts.Display.Views.Refresh();

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information,
                    $"{numberOfParts} {componentPrefix}s added, and expression 'Dim' set to {dimValue}.");
            }
            catch (Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "Failed: " + ex.Message);
            }
        }



    }
}