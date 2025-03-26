using NXOpen;
using NXOpen.Annotations;
using NXOpen.Drawings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRAC_FORM
{
    public partial class Form2_PICATINNY : Form
    {
        public Form2_PICATINNY()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string Width = textBox1.Text;
            string Parallax = textBox2.Text;
            Point3d position = new Point3d(0.0, 0.0, 0.0);
            string D_width = GlobalVariables.PipeDiameter;
            string partsFolderPath = GlobalVariables.FilePath;

            Class_Add_item addItem = new Class_Add_item();

            addItem.AddPartToAssembly("Lower_brac_Picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Upper_brac_Picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Skruvar_picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Parralax_distans.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + Parallax, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Picatinny_SAT.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + Parallax, position, partsFolderPath, assemblyPart);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Picatinny Bracket added.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1_PICATINNY form1 = new Form1_PICATINNY();
            form1.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Class_Add_item addItem = new Class_Add_item();

            addItem.DeleteBracket("Lower_brac_Picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Upper_brac_Picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Skruvar_picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Parralax_distans", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Picatinny_SAT", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Picatinny_rail", GlobalVariables.bracketCounter);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket was removed from assembly");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part workPart = theSession.Parts.Work;
            Part displayPart = theSession.Parts.Display;

            // Skapa assemblyritning
            FileNew fileNew1 = theSession.Parts.FileNew();
            fileNew1.TemplateFileName = "sts-A3.prt";
            fileNew1.UseBlankTemplate = false;
            fileNew1.ApplicationName = "DrawingTemplate";
            fileNew1.Units = Part.Units.Millimeters;
            fileNew1.UsesMasterModel = "Yes";
            fileNew1.TemplateType = FileNewTemplateType.Item;
            fileNew1.TemplatePresentationName = "STS - A3";
            fileNew1.NewFileName = @"C:\\Users\\u107284\\Desktop\\REPPOE\\BRAC_FORM\\CAD\\assembly1_dwg1.prt";
            fileNew1.MasterFileName = "assembly1";
            fileNew1.MakeDisplayedPart = true;
            fileNew1.DisplayPartOption = DisplayPartOption.AllowAdditional;
            fileNew1.Commit();
            fileNew1.Destroy();

            workPart = theSession.Parts.Work;
            workPart.Drafting.EnterDraftingApplication();
            workPart.Drafting.SetTemplateInstantiationIsComplete(true);

            // === FRONT ===
            BaseViewBuilder frontBuilder = workPart.DraftingViews.CreateBaseViewBuilder(null);
            frontBuilder.SelectModelView.SelectedView = workPart.ModelingViews.FindObject("Front") as ModelingView;
            frontBuilder.Placement.Placement.SetValue(null, workPart.Views.WorkView, new Point3d(100, 100, 0));
            BaseView frontView = (BaseView)frontBuilder.Commit();
            frontBuilder.Destroy();

            // === RIGHT ===
            BaseViewBuilder rightBuilder = workPart.DraftingViews.CreateBaseViewBuilder(null);
            rightBuilder.SelectModelView.SelectedView = workPart.ModelingViews.FindObject("Right") as ModelingView;
            rightBuilder.Placement.Placement.SetValue(frontView, workPart.Views.WorkView, new Point3d(220, 100, 0));
            BaseView rightView = (BaseView)rightBuilder.Commit();
            rightBuilder.Destroy();

            // === ISOMETRIC ===
            BaseViewBuilder isoBuilder = workPart.DraftingViews.CreateBaseViewBuilder(null);
            isoBuilder.SelectModelView.SelectedView = workPart.ModelingViews.FindObject("Isometric") as ModelingView;
            isoBuilder.Placement.Placement.SetValue(null, workPart.Views.WorkView, new Point3d(210, 200, 0));
            BaseView isoView = (BaseView)isoBuilder.Commit();
            isoBuilder.Destroy();

            // Parts list
            PlistBuilder plistBuilder = workPart.Annotations.PartsLists.CreatePartsListBuilder(null);
            plistBuilder.Origin.Plane.PlaneMethod = NXOpen.Annotations.PlaneBuilder.PlaneMethodType.XyPlane;
            plistBuilder.Contents.TopLevelAssembly = NXOpen.Annotations.PartsListContentsBuilder.TopLevelAssemblyType.ChildPart;
            plistBuilder.Origin.Origin.SetValue(null, null, new Point3d(300, 270, 0));
            plistBuilder.Commit();
            plistBuilder.Destroy();

            // === Skapa separata detaljritningar i nya NX-fönster ===
            CreateSeparateDetailDrawing("Lower_brac_Picatinny_1");
            CreateSeparateDetailDrawing("Upper_brac_Picatinny_1");

            MessageBox.Show("Alla ritningar skapade!");
            
            //theSession.ApplicationSwitchImmediate("UG_APP_MODELING"); Denna ska fixa saker men fixar inget

     
        }

        private void CreateSeparateDetailDrawing(string partName)
        {
            Session theSession = Session.GetSession();
            string baseDir = @"C:\Users\u107284\Desktop\REPPOE\BRAC_FORM\CAD\";
            string partPath = baseDir + partName + ".prt";
            string drawingPath = baseDir + partName + "_dwg1.prt";

            Part detailPart = null;

            // 🔍 Kontrollera om parten redan är öppen
            foreach (Part p in theSession.Parts.ToArray())
            {
                if (p.FullPath.ToLower() == partPath.ToLower())
                {
                    detailPart = p;
                    break;
                }
            }

            // 📂 Om inte öppen, öppna i nytt fönster
            if (detailPart == null)
            {
                PartLoadStatus loadStatus;
                detailPart = theSession.Parts.OpenDisplay(partPath, out loadStatus);
                loadStatus.Dispose();
            }

            //Skapa ritningsfil för detaljen
            FileNew fileNew = theSession.Parts.FileNew();
            fileNew.TemplateFileName = "sts-A3.prt";
            fileNew.TemplatePresentationName = "STS - A3";
            fileNew.TemplateType = FileNewTemplateType.Item;
            fileNew.ApplicationName = "DrawingTemplate";
            fileNew.Units = Part.Units.Millimeters;
            fileNew.NewFileName = drawingPath;
            fileNew.MasterFileName = partPath;
            fileNew.UseBlankTemplate = false;
            fileNew.MakeDisplayedPart = true;
            fileNew.DisplayPartOption = DisplayPartOption.AllowAdditional;
            fileNew.Commit();
            fileNew.Destroy();

            Part drawingPart = theSession.Parts.Work;
            drawingPart.Drafting.EnterDraftingApplication();
            drawingPart.Drafting.SetTemplateInstantiationIsComplete(true);

            // === Vyer ===ssssssssssssss
            ModelingView front = drawingPart.ModelingViews.FindObject("Front") as ModelingView;
            ModelingView right = drawingPart.ModelingViews.FindObject("Right") as ModelingView;
            ModelingView iso = drawingPart.ModelingViews.FindObject("Isometric") as ModelingView;

            BaseViewBuilder frontBuilder = drawingPart.DraftingViews.CreateBaseViewBuilder(null);
            frontBuilder.SelectModelView.SelectedView = front;
            frontBuilder.Placement.Placement.SetValue(null, drawingPart.Views.WorkView, new Point3d(100, 100, 0));
            BaseView frontView = (BaseView)frontBuilder.Commit();
            frontBuilder.Destroy();

            BaseViewBuilder rightBuilder = drawingPart.DraftingViews.CreateBaseViewBuilder(null);
            rightBuilder.SelectModelView.SelectedView = right;
            rightBuilder.Placement.Placement.SetValue(frontView, drawingPart.Views.WorkView, new Point3d(220, 100, 0));
            BaseView rightView = (BaseView)rightBuilder.Commit();
            rightBuilder.Destroy();

            BaseViewBuilder isoBuilder = drawingPart.DraftingViews.CreateBaseViewBuilder(null);
            isoBuilder.SelectModelView.SelectedView = iso;
            isoBuilder.Placement.Placement.SetValue(null, drawingPart.Views.WorkView, new Point3d(250, 210, 0));
            BaseView isoView = (BaseView)isoBuilder.Commit();
            isoBuilder.Destroy();
            
        }




        private void Form2_PICATINNY_Load(object sender, EventArgs e)
        {
        }
    }
}