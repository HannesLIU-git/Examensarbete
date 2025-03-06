using NXOpen;
using NXOpen.Assemblies;
using NXOpen.CAE;
using System;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using static NXOpen.Selection;

namespace BRAC_FORM
{
    public class Class_Add_item
    {

        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private int counter;

        public void DeleteBracket(string partNameToDelete, int counter)
        {
            try {
                NXOpen.Session theSession = NXOpen.Session.GetSession();
                NXOpen.Part workPart = theSession.Parts.Work;
                NXOpen.Part displayPart = theSession.Parts.Display;

                NXOpen.Session.UndoMarkId markId1;

                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Invisible, "Delete");

                theSession.UpdateManager.ClearErrorList();

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Delete");

                NXOpen.TaggedObject[] objects1 = new NXOpen.TaggedObject[1];
                NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT {partNameToDelete}_{counter} 1"));
                objects1[0] = component1;
                int nErrs1;
                nErrs1 = theSession.UpdateManager.AddObjectsToDeleteList(objects1);

                NXOpen.Session.UndoMarkId id1;
                id1 = theSession.NewestVisibleUndoMark;

                int nErrs2;
                nErrs2 = theSession.UpdateManager.DoUpdate(id1);

                theSession.DeleteUndoMark(markId1, null);

                string partsFolderPath = GlobalVariables.FilePath;

                string newPartPath = Path.Combine(Path.GetDirectoryName(partsFolderPath), $"{partNameToDelete}_{counter}.prt");

                File.Delete(newPartPath);
            }

            catch (Exception ex)
            {
                UI.GetUI().NXMessageBox.Show("Error", NXMessageBox.DialogType.Error, "Failed to delete part: " + ex.Message);
            }

        }

        

        public void HideDatumsAndSketches()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Edit->Show and Hide->Show and Hide...
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Start");

            theSession.SetUndoMarkName(markId1, "Show and Hide Dialog");

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Hide Sketches");

            int numberHidden1;
            numberHidden1 = theSession.DisplayManager.HideByType("SHOW_HIDE_TYPE_SKETCHES", NXOpen.DisplayManager.ShowHideScope.AnyInAssembly);

            int nErrs1;
            nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

            workPart.ModelingViews.WorkView.FitAfterShowOrHide(NXOpen.View.ShowOrHideType.HideOnly);

            NXOpen.Session.UndoMarkId markId3;
            markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Hide Datums");

            int numberHidden2;
            numberHidden2 = theSession.DisplayManager.HideByType("SHOW_HIDE_TYPE_DATUMS", NXOpen.DisplayManager.ShowHideScope.AnyInAssembly);

            int nErrs2;
            nErrs2 = theSession.UpdateManager.DoUpdate(markId3);

            workPart.ModelingViews.WorkView.FitAfterShowOrHide(NXOpen.View.ShowOrHideType.HideOnly);

            theSession.SetUndoMarkName(markId1, "Show and Hide");

            theSession.DeleteUndoMark(markId1, null);

            theSession.CleanUpFacetedFacesAndEdges();
        }
        public void updateAll()
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            // ----------------------------------------------
            //   Menu: Tools->Update->Interpart Update->Update All
            // ----------------------------------------------
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Update Session");

            theSession.UpdateManager.DoInterpartUpdate(markId1);

            theSession.UpdateManager.DoAssemblyConstraintsUpdate(markId1);

        }

        //public Class_Add_item(TextBox tb5, TextBox tb6, TextBox tb7, TextBox tb8, int counter)
        //{
        //    textBox5 = tb5;
        //    textBox6 = tb6;
        //    textBox7 = tb7;
        //    textBox8 = tb8;
        //    this.counter = counter;
        //}

       
        public void AddPartToAssembly(string partName,int counter, string dimensions, Point3d position, string partsFolderPath, Part assemblyPart)
        {
            try
            {
                // Get the current session
                Session theSession = Session.GetSession();

                string partPrefix = partName.Substring(0, partName.Length - 4); //Removes .prt

                // Construct the full path for the part
                string partFilePath = Path.Combine(partsFolderPath, partName); //MASTERPATH

                string newPartPath = Path.Combine(Path.GetDirectoryName(partFilePath), $"{partPrefix}_{counter}.prt");

                File.Copy(partFilePath, newPartPath, true);

                // Open the part to be added
                PartLoadStatus loadStatus;
                Part part = (Part)theSession.Parts.OpenBase(newPartPath, out loadStatus); // Open part
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

                theSession.Parts.Display.Views.Refresh();

                // UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Part added at origin.");
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

                // Ensure the first value is D_width and the second value is Width
                string D_widthValue = dimensionValues[0];
                //string WidthValue = dimensionValues.Length > 1 ? dimensionValues[1] : "1.0"; // Default to 1.0 if no second value
                //string XPosValue = dimensionValues.Length > 2 ? dimensionValues[2] : "1.0"; // Default to 1.0 if no second value
                //string YPosValue = dimensionValues.Length > 3 ? dimensionValues[3] : "1.0"; // Default to 1.0 if no second valu

                // Update or create the "D_width" expression for the part
                Expression D_widthExpression;
                try
                {
                    D_widthExpression = part.Expressions.FindObject("D_width");
                }
                catch
                {
                    // Create the expression if it doesn't exist
                    D_widthExpression = part.Expressions.CreateExpression("D_width", D_widthValue); // Directly use the string value
                }
                D_widthExpression.RightHandSide = D_widthValue; // Directly use the string value

                // Update or create the "Width" expression for the part (if needed)
                if (dimensionValues.Length > 1)
                {
                    string WidthValue = dimensionValues[1];
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
                }


                if (dimensionValues.Length > 2)
                {
                    string XPosValue = dimensionValues[2];
                    Expression XPosExpression;
                    try
                    {
                        XPosExpression = part.Expressions.FindObject("X_pos");
                    }
                    catch
                    {
                        // Create the expression if it doesn't exist
                        XPosExpression = part.Expressions.CreateExpression("X_pos", XPosValue); // Directly use the string value
                    }
                    XPosExpression.RightHandSide = XPosValue; // Directly use the string value
                }

                if (dimensionValues.Length > 3)
                {
                    string YPosValue = dimensionValues[3];
                    Expression YPosExpression;
                    try
                    {
                        YPosExpression = part.Expressions.FindObject("Y_pos");
                    }
                    catch
                    {
                        // Create the expression if it doesn't exist
                        YPosExpression = part.Expressions.CreateExpression("Y_pos", YPosValue); // Directly use the string value
                    }
                    YPosExpression.RightHandSide = YPosValue; // Directly use the string value
                }

                if (dimensionValues.Length > 4)
                {
                    string Gaffel_WValue = dimensionValues[4];
                    Expression Gaffel_WExpression;
                    try
                    {
                        Gaffel_WExpression = part.Expressions.FindObject("Gaffel_W");
                    }
                    catch
                    {
                        // Create the expression if it doesn't exist
                        Gaffel_WExpression = part.Expressions.CreateExpression("Gaffel_W", Gaffel_WValue); // Directly use the string value
                    }
                    Gaffel_WExpression.RightHandSide = Gaffel_WValue; // Directly use the string value
                }

                if (dimensionValues.Length > 5)
                {
                    string Gaffel_LValue = dimensionValues[5];
                    Expression Gaffel_LExpression;
                    try
                    {
                        Gaffel_LExpression = part.Expressions.FindObject("Gaffel_L");
                    }
                    catch
                    {
                        // Create the expression if it doesn't exist
                        Gaffel_LExpression = part.Expressions.CreateExpression("Gaffel_L", Gaffel_LValue); // Directly use the string value
                    }
                    Gaffel_LExpression.RightHandSide = Gaffel_LValue; // Directly use the string value
                }
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

