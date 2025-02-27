using NXOpen;
using System.Collections.Generic;
using System;

public class CreatePoint1
{
    // Static variable to persist the counter across different instances and function calls
    private static int globalCounter = 0;

    private static Session theSession = null;
    private static UI theUI = null;
    private string theDlxFileName;
    private NXOpen.BlockStyler.BlockDialog theDialog;
    private NXOpen.BlockStyler.Group group0;
    private NXOpen.BlockStyler.SpecifyPoint selected_point; // Block type: Specify Point
    private NXOpen.BlockStyler.Button point_button; // Block type: Button

    //Bit Option for Property: SnapPointTypesEnabled
    //------------------------------------------------------------------------------
    public static readonly int SnapPointTypesEnabled_UserDefined = (1 << 0);
    public static readonly int SnapPointTypesEnabled_Inferred = (1 << 1);
    public static readonly int SnapPointTypesEnabled_ScreenPosition = (1 << 2);
    public static readonly int SnapPointTypesEnabled_EndPoint = (1 << 3);
    public static readonly int SnapPointTypesEnabled_MidPoint = (1 << 4);
    public static readonly int SnapPointTypesEnabled_ControlPoint = (1 << 5);
    public static readonly int SnapPointTypesEnabled_Intersection = (1 << 6);
    public static readonly int SnapPointTypesEnabled_ArcCenter = (1 << 7);
    public static readonly int SnapPointTypesEnabled_QuadrantPoint = (1 << 8);
    public static readonly int SnapPointTypesEnabled_ExistingPoint = (1 << 9);
    public static readonly int SnapPointTypesEnabled_PointonCurve = (1 << 10);
    public static readonly int SnapPointTypesEnabled_PointonSurface = (1 << 11);
    public static readonly int SnapPointTypesEnabled_PointConstructor = (1 << 12);
    public static readonly int SnapPointTypesEnabled_TwocurveIntersection = (1 << 13);
    public static readonly int SnapPointTypesEnabled_TangentPoint = (1 << 14);
    public static readonly int SnapPointTypesEnabled_Poles = (1 << 15);
    public static readonly int SnapPointTypesEnabled_BoundedGridPoint = (1 << 16);
    public static readonly int SnapPointTypesEnabled_FacetVertexPoint = (1 << 17);
    public static readonly int SnapPointTypesEnabled_DefiningPoint = (1 << 18);
    //------------------------------------------------------------------------------
    //Bit Option for Property: SnapPointTypesOnByDefault
    //------------------------------------------------------------------------------
    public static readonly int SnapPointTypesOnByDefault_UserDefined = (1 << 0);
    public static readonly int SnapPointTypesOnByDefault_Inferred = (1 << 1);
    public static readonly int SnapPointTypesOnByDefault_ScreenPosition = (1 << 2);
    public static readonly int SnapPointTypesOnByDefault_EndPoint = (1 << 3);
    public static readonly int SnapPointTypesOnByDefault_MidPoint = (1 << 4);
    public static readonly int SnapPointTypesOnByDefault_ControlPoint = (1 << 5);
    public static readonly int SnapPointTypesOnByDefault_Intersection = (1 << 6);
    public static readonly int SnapPointTypesOnByDefault_ArcCenter = (1 << 7);
    public static readonly int SnapPointTypesOnByDefault_QuadrantPoint = (1 << 8);
    public static readonly int SnapPointTypesOnByDefault_ExistingPoint = (1 << 9);
    public static readonly int SnapPointTypesOnByDefault_PointonCurve = (1 << 10);
    public static readonly int SnapPointTypesOnByDefault_PointonSurface = (1 << 11);
    public static readonly int SnapPointTypesOnByDefault_PointConstructor = (1 << 12);
    public static readonly int SnapPointTypesOnByDefault_TwocurveIntersection = (1 << 13);
    public static readonly int SnapPointTypesOnByDefault_TangentPoint = (1 << 14);
    public static readonly int SnapPointTypesOnByDefault_Poles = (1 << 15);
    public static readonly int SnapPointTypesOnByDefault_BoundedGridPoint = (1 << 16);
    public static readonly int SnapPointTypesOnByDefault_FacetVertexPoint = (1 << 17);
    public static readonly int SnapPointTypesOnByDefault_DefiningPoint = (1 << 18);


    // Constructor for NX Styler class
    public CreatePoint1()
    {
        try
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theDlxFileName = "C:\\Users\\hanli255\\source\\repos\\from-weapon-brac\\BRAC_FORM\\Point_UI.dlx";
            theDialog = theUI.CreateDialog(theDlxFileName);
            theDialog.AddApplyHandler(new NXOpen.BlockStyler.BlockDialog.Apply(apply_cb));
            theDialog.AddOkHandler(new NXOpen.BlockStyler.BlockDialog.Ok(ok_cb));
            theDialog.AddUpdateHandler(new NXOpen.BlockStyler.BlockDialog.Update(update_cb));
            theDialog.AddInitializeHandler(new NXOpen.BlockStyler.BlockDialog.Initialize(initialize_cb));
            theDialog.AddDialogShownHandler(new NXOpen.BlockStyler.BlockDialog.DialogShown(dialogShown_cb));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static int GetCounter()
    {
        return globalCounter;
    }

    // Method to create the point, using the global counter
    public double[] createpoint()
    {
        // Retrieve the point selected by the user from the SpecifyPoint block

        // Get the active part
        NXOpen.Part workPart = theSession.Parts.Work;
        NXOpen.Part displayPart = theSession.Parts.Display;

        double[] createdPointsCoords = new double[3];

        // Loop to create a point
        Point3d selectedPoint = selected_point.Point; // Get the user-selected point
        // Create the point in the model
        Point nxPoint = workPart.Points.CreatePoint(selectedPoint);

        double[] pointAsDouble = new double[] { selectedPoint.X, selectedPoint.Y, selectedPoint.Z };

        // Store the coordinates

        createdPointsCoords[0] = pointAsDouble[0];
        createdPointsCoords[1] = pointAsDouble[1];
        createdPointsCoords[2] = pointAsDouble[2];

        // Store the created point with the current counter
        InitialPoints[globalCounter] = createdPointsCoords;

        // Update globalCounter


        // Show information
        theUI.NXMessageBox.Show("Point Created", NXMessageBox.DialogType.Information,
                    $"Point {globalCounter} created at ({selectedPoint.X}, {selectedPoint.Y}, {selectedPoint.Z})");

        globalCounter++;
        // Refresh the view
        theSession.Parts.Display.Views.Refresh();

        return createdPointsCoords;
    }

    // Dictionary to store points
    public Dictionary<int, double[]> InitialPoints = new Dictionary<int, double[]>();



    // The Apply callback that is triggered when Apply is clicked in the UI
    public int apply_cb()
    {
        int errorCode = 0;
        try
        {
            // Use the static globalCounter
            createpoint();
        }
        catch (Exception ex)
        {
            errorCode = 1;
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return errorCode;
    }

    // Other callback methods...
    public int update_cb(NXOpen.BlockStyler.UIBlock block) { return 0; }
    public int ok_cb() { return apply_cb(); }
    public void initialize_cb()
    {
        try
        {
            group0 = (NXOpen.BlockStyler.Group)theDialog.TopBlock.FindBlock("group0");
            selected_point = (NXOpen.BlockStyler.SpecifyPoint)theDialog.TopBlock.FindBlock("selected_point");
            point_button = (NXOpen.BlockStyler.Button)theDialog.TopBlock.FindBlock("point_button");
        }
        catch (Exception ex)
        {
            //---- Enter your exception handling code here -----
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
    public void dialogShown_cb() { }

    // Show the dialog
    public NXOpen.UIStyler.DialogResponse Show()
    {
        try
        {
            theDialog.Show();
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
        return 0;
    }

    // Dispose the dialog
    public void Dispose()
    {
        if (theDialog != null)
        {
            theDialog.Dispose();
            theDialog = null;
        }
    }

    // Get the unload option
    public static int GetUnloadOption(string arg)
    {
        return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);
    }

    // Cleanup
    public static void UnloadLibrary(string arg) { }

    // Main entry for the Point_UI functionality
    public static void Run()
    {
        try
        {
            // Directly create an instance and invoke createpoint directly from a button click or event
            CreatePoint1 pointUI = new CreatePoint1();
            pointUI.Show();
        }
        catch (Exception ex)
        {
            theUI.NXMessageBox.Show("Block Styler", NXMessageBox.DialogType.Error, ex.ToString());
        }
    }
}
