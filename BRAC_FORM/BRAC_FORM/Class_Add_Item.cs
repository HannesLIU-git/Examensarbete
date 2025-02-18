using NXOpen;
using NXOpen.Assemblies;
using System;
using System.IO;
using System.Windows.Forms;

namespace BRAC_FORM
{
    public class Class_Add_item
    {
        private TextBox textBox5;
        private TextBox textBox6;

        public Class_Add_item(TextBox tb5, TextBox tb6)
        {
            textBox5 = tb5;
            textBox6 = tb6;
        }

        public void AddThreeParts()
        {
            try
            {
                // Get the current session and open the assembly
                Session theSession = Session.GetSession();
                Part assemblyPart = (Part)theSession.Parts.Work;

                if (assemblyPart == null || assemblyPart.ComponentAssembly == null)
                {
                    UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "No assembly is currently open as the work part.");
                    return;
                }

                // TextBox values for dimensioning
                string D_pipa = textBox5.Text;
                string Width = textBox6.Text;

                // Parts folder path
                string partsFolderPath = @"C:\Users\hanli255\source\repos\from-weapon-brac\BRAC_FORM\CAD\";

                // Position the parts at the origin
                Point3d position = new Point3d(0.0, 0.0, 0.0);

                // Add "Pipa.prt"
                AddPartToAssembly("Pipa.prt", D_pipa, position, partsFolderPath, assemblyPart);

                // Move position for next part
                position.X += 0;

                // Add "LowerBrac.prt"
                AddPartToAssembly("LowerBrac.prt", D_pipa + "," + Width, position, partsFolderPath, assemblyPart);

                // Move position for next partnx

                position.X += 0;
                // Add "Upper_brac.prt"
                AddPartToAssembly("Upper_brac.prt", D_pipa + "," + Width, position, partsFolderPath, assemblyPart);

                // Refresh the view after adding parts
                theSession.Parts.Display.Views.Refresh();

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Three parts added at origin.");

            }
            catch (Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "Failed to add parts: " + ex.Message);
            }
        }
        private void AddPartToAssembly(string partName, string dimensions, Point3d position, string partsFolderPath, Part assemblyPart)
        {
            try
            {
                // Get the current session
                Session theSession = Session.GetSession();

                // Construct the full path for the part
                string partFilePath = Path.Combine(partsFolderPath, partName);

                // Open the part to be added
                PartLoadStatus loadStatus;
                Part part = (Part)theSession.Parts.OpenBase(partFilePath, out loadStatus); // Open part
                loadStatus.Dispose(); // Dispose loadStatus once it's used

                // Create the add component builder
                AddComponentBuilder addComponentBuilder = assemblyPart.AssemblyManager.CreateAddComponentBuilder();
                addComponentBuilder.SetPartsToAdd(new BasePart[] { part });
                addComponentBuilder.ComponentName = partName;
                addComponentBuilder.ReferenceSet = "Entire Part";

                // Define the orientation matrix (ensure it's aligned)
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

                // Set all components to be placed at the origin (0, 0, 0)
                addComponentBuilder.SetInitialLocationAndOrientation(position, orientation);

                // Commit the part to the assembly
                Component newComponent = addComponentBuilder.Commit() as Component;
                addComponentBuilder.Destroy();

                // Now, we will handle the user expression for each part based on the dimensions
                UpdatePartExpression(part, dimensions, assemblyPart);

            }
            catch (Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "Failed to add part: " + ex.Message);
            }
        }
        private void UpdatePartExpression(Part part, string dimensions, Part assemblyPart)
        {
            try
            {
                Session theSession = Session.GetSession();

                // Split dimensions to handle multiple parameters
                string[] dimensionValues = dimensions.Split(',');

                // Ensure the first value is D_pipa and the second value is Width
                string D_pipaValue = dimensionValues[0];
                string WidthValue = dimensionValues.Length > 1 ? dimensionValues[1] : "1.0"; // Default to 1.0 if no second value

                // Update or create the "D_pipa" expression for the part
                Expression D_pipaExpression;
                try
                {
                    D_pipaExpression = part.Expressions.FindObject("D_pipa");
                }
                catch
                {
                    // Create the expression if it doesn't exist
                    D_pipaExpression = part.Expressions.CreateExpression("D_pipa", D_pipaValue); // Directly use the string value
                }
                D_pipaExpression.RightHandSide = D_pipaValue; // Directly use the string value

                // Update or create the "Width" expression for the part (if needed)
                Expression WidthExpression;
                try
                {
                    WidthExpression = part.Expressions.FindObject("Width");
                }
                catch
                {
                    // Create the expression if it doesn't exist
                    WidthExpression = part.Expressions.CreateExpression("Width", WidthValue); // Directly use the string value
                }
                WidthExpression.RightHandSide = WidthValue; // Directly use the string value

                // Refresh the part to reflect changes
               theSession.Parts.Display.Views.Refresh();
            }
            catch (Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "Failed to update expressions: " + ex.Message);
            }
        }




       




    }





}

