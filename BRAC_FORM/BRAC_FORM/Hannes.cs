using NXOpen;
using NXOpen.Assemblies;
using NXOpen.Positioning;
using System;
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

        public void AddMultipleCubesAndSetExpression()
        {
            try
            {
                Session theSession = Session.GetSession();

                string asmPath = @"C:\Users\hanli255\source\Repos\from-weapon-brac\BRAC_FORM\CAD\asm_test.prt";
                string partPath = @"C:\Users\hanli255\source\Repos\from-weapon-brac\BRAC_FORM\CAD\KUB.prt";

                // Open the assembly
                PartLoadStatus loadStatusOpen;
                BasePart basePart = theSession.Parts.Open(asmPath, out loadStatusOpen);
                Part assemblyPart = (Part)basePart;

                PartLoadStatus loadStatusSetDisplay;
                theSession.Parts.SetDisplay(basePart, false, false, out loadStatusSetDisplay);
                theSession.Parts.SetWork(assemblyPart);

                loadStatusOpen.Dispose();
                loadStatusSetDisplay.Dispose();

                // Get ComponentAssembly object
                ComponentAssembly componentAssembly = assemblyPart.ComponentAssembly;

                // Open the part to add once
                PartLoadStatus loadStatusPart;
                BasePart partToAdd = theSession.Parts.Open(partPath, out loadStatusPart);
                loadStatusPart.Dispose();

                // Convert user input to number of cubes
                int numberOfCubes = int.Parse(textBox1.Text);

                double xOffset = 0.0;

                for (int i = 0; i < numberOfCubes; i++)
                {
                    AddComponentBuilder addComponentBuilder = assemblyPart.AssemblyManager.CreateAddComponentBuilder();

                    BasePart[] partsToAdd = new BasePart[] { partToAdd };
                    addComponentBuilder.SetPartsToAdd(partsToAdd);

                    addComponentBuilder.ComponentName = $"KUB_{i + 1}";
                    addComponentBuilder.ReferenceSet = "Model";

                    Point3d position = new Point3d(xOffset, 0.0, 0.0);
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

                    addComponentBuilder.Commit();
                    addComponentBuilder.Destroy();

                    xOffset += 110.0;
                }

                // Update the expression called "Dim" with the value from textBox2
                string dimValue = textBox2.Text;

                Expression dimExpression;
                try
                {
                    dimExpression = assemblyPart.Expressions.FindObject("dim");
                }
                catch
                {
                    dimExpression = assemblyPart.Expressions.CreateExpression("Number", "dim=1.0");
                }

                dimExpression.RightHandSide = dimValue;


                theSession.Parts.Display.Views.Refresh();

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information,
                    $"{numberOfCubes} cubes added, and expression 'Dim' set to {dimValue}.");
            }
            catch (Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "Failed: " + ex.Message);
            }
        }
    }
}
