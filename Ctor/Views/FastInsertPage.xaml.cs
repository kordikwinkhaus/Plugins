using System.Windows.Controls;
using System.Xml.Linq;
using Ctor.ViewModels;
using UserExtensions;
using WHOkna;

namespace Ctor.Views
{
    public partial class FastInsertPage : UserControl, IUserForm3
    {
        public FastInsertPage()
        {
            InitializeComponent();
        }

        public IDataRequest DataCallback { get; set; }

        public XElement ObjectData { get; set; }

        private IOknaDocument _oknaDoc;
        public IOknaDocument OknaDoc
        {
            get { return ViewModel.Document; }
            set { ViewModel.Document = value; }
        }

        public IPart Part { get; set; }

        public object Title
        {
            get { return Properties.Resources.FastInsertPageTitle; }
        }

        private FastInsertViewModel _viewModel;
        internal FastInsertViewModel ViewModel
        {
            get { return _viewModel; }
            set { DataContext = _viewModel = value; }
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            return true;
        }

        private void Clear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.RunCommand.Execute("if pos.IsConstruction: pos.Clear()");
        }

        private void Mirror_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.RunCommand.Execute("if pos.IsConstruction: pos.Mirror()");
        }

        private void Fix_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtCode.Text = @"if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 300:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 300:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket

    frame = area.InsertFrame(typeID, colorID)
    frame.InsertGlasspackets(glasspacket)";
        }

        private void FixVS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FixS("Vertical");
        }

        private void FixHS_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FixS("Horizontal");
        }

        private void FixS(string dir)
        {
            txtCode.Text = @"if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 300:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 300:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket
    type = db.GetWindowType(typeID)
    mullionNrArt = type.Mullions." + dir + @".Frame.Default

    frame = area.InsertFrame(typeID, colorID)
    frame.Insert" + dir + @"Mullion(mullionNrArt)
    frame.InsertGlasspackets(glasspacket)";
        }

        private void _1kl_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _1kl("False");
        }

        private void _1kp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _1kl("True");
        }

        private void _1kl(string dir)
        {
            txtCode.Text = @"if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 400:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 500:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket
    fittingsGroup = ctx.FittingsGroup

    frame = area.InsertFrame(typeID, colorID)
    sash = frame.InsertSash()
    sash.InsertGlasspackets(glasspacket)
    if ctx.InsertFittings:
        args = sash.GetFittingsFindArgs()
        fittingsTypeID = db.FindFittingsType(fittingsGroup, args)
        sash.InsertFittings(fittingsTypeID, " + dir + ")";
        }

        private void _1ks_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtCode.Text = @"if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 400:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 500:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket
    fittingsGroup = ctx.FittingsGroup

    frame = area.InsertFrame(typeID, colorID)
    sash = frame.InsertSash()
    sash.InsertGlasspackets(glasspacket)
    if ctx.InsertFittings:
        args = sash.GetFittingsFindArgs()
        args.TiltOnly = True
        fittingsTypeID = db.FindFittingsType(fittingsGroup, args)
        sash.InsertFittings(fittingsTypeID)";
        }

        private void _2kl_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _2kl("True");
        }

        private void _2kp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _2kl("False");
        }

        private void _2kl(string dir)
        {
            txtCode.Text = @"if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 900:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 500:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket
    type = db.GetWindowType(typeID)
    fittingsGroup = ctx.FittingsGroup

    frame = area.InsertFrame(typeID, colorID)
    frame.InsertFalseMullion(type.DefaultFalseMullion, " + dir + @")
    frame.InsertGlasspackets(glasspacket)
    if ctx.InsertFittings:
        for sash in frame.GetSashes():
            args = sash.GetFittingsFindArgs()
            fittingsTypeID = db.FindFittingsType(fittingsGroup, args)
            sash.InsertFittings(fittingsTypeID)";
        }

        private void _3kl_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtCode.Text = @"import clr
clr.AddReference('Okna.Data')
from Okna.Data import DatabaseCommand

if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 1200:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 500:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket
    type = db.GetWindowType(typeID)
    fittingsGroup = ctx.FittingsGroup
    mullionNrArt = type.Mullions.Vertical.Frame.Default
    cmd = DatabaseCommand()
    cmd.CommandText = '''SELECT (SELECT(w1*2)-w3 FROM dbo.osciezp WHERE nr_art=@ram) AS Ram,
(SELECT w1-w2 FROM dbo.przymyki WHERE nr_art=@stulp) AS Stulp,
(SELECT w1-w3 FROM dbo.slupki WHERE nr_art=@sloupek) AS Sloupek'''
    cmd.AddParameter('@ram', type.GetField('osciez1'))
    cmd.AddParameter('@sloupek', mullionNrArt)
    cmd.AddParameter('@stulp', type.DefaultFalseMullion)
    korekce = db.ExecuteQuery(cmd)[0]

    frame = area.InsertFrame(typeID, colorID)
    dims = frame.GetCorrectedDimensions()
    kridlo = (dims.Width - 2 * (korekce.Ram + korekce.Stulp + korekce.Sloupek)) / 3.0
    mullionX = dims.Left +  korekce.Ram + kridlo + korekce.Sloupek
    falseMullionX = dims.Right - korekce.Ram - kridlo - korekce.Stulp

    mr = frame.InsertVerticalMullion(mullionNrArt, mullionX)
    mr.Area2.InsertFalseMullion(type.DefaultFalseMullion, False, falseMullionX)
    frame.InsertSashes()
    frame.InsertGlasspackets(glasspacket)
    if ctx.InsertFittings:
        for sash in frame.GetSashes():
            args = sash.GetFittingsFindArgs()
            fittingsTypeID = db.FindFittingsType(fittingsGroup, args)
            sash.InsertFittings(fittingsTypeID)";
        }

        private void _3kp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtCode.Text = @"import clr
clr.AddReference('Okna.Data')
from Okna.Data import DatabaseCommand

if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 1200:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 500:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket
    type = db.GetWindowType(typeID)
    fittingsGroup = ctx.FittingsGroup
    mullionNrArt = type.Mullions.Vertical.Frame.Default
    cmd = DatabaseCommand()
    cmd.CommandText = '''SELECT (SELECT(w1*2)-w3 FROM dbo.osciezp WHERE nr_art=@ram) AS Ram,
(SELECT w1-w2 FROM dbo.przymyki WHERE nr_art=@stulp) AS Stulp,
(SELECT w1-w3 FROM dbo.slupki WHERE nr_art=@sloupek) AS Sloupek'''
    cmd.AddParameter('@ram', type.GetField('osciez1'))
    cmd.AddParameter('@sloupek', mullionNrArt)
    cmd.AddParameter('@stulp', type.DefaultFalseMullion)
    korekce = db.ExecuteQuery(cmd)[0]

    frame = area.InsertFrame(typeID, colorID)
    dims = frame.GetCorrectedDimensions()
    kridlo = (dims.Width - 2 * (korekce.Ram + korekce.Stulp + korekce.Sloupek)) / 3.0
    mullionX = dims.Right - korekce.Ram - kridlo - korekce.Sloupek
    falseMullionX = dims.Left + korekce.Ram + kridlo + korekce.Stulp

    mr = frame.InsertVerticalMullion(mullionNrArt, mullionX)
    mr.Area1.InsertFalseMullion(type.DefaultFalseMullion, True, falseMullionX)
    frame.InsertSashes()
    frame.InsertGlasspackets(glasspacket)
    if ctx.InsertFittings:
        for sash in frame.GetSashes():
            args = sash.GetFittingsFindArgs()
            fittingsTypeID = db.FindFittingsType(fittingsGroup, args)
            if sash.ID == 3:
                sash.InsertFittings(fittingsTypeID, True)
            else:
                sash.InsertFittings(fittingsTypeID)";
        }

        private void fkslv_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtCode.Text = @"if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 1000:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 600:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket
    type = db.GetWindowType(typeID)
    fittingsGroup = ctx.FittingsGroup
    mullionFrameVerNrArt = type.Mullions.Vertical.Frame.Default
    mullionFrameHorNrArt = type.Mullions.Horizontal.Frame.Default
    mullionSashNrArt = type.Mullions.Horizontal.Sash.Default

    frame = area.InsertFrame(typeID, colorID)
    m1 = frame.InsertVerticalMullion(mullionFrameVerNrArt)
    sash = m1.Area2.InsertSash()
    m2 = m1.Area1.InsertHorizontalMullion(mullionFrameHorNrArt)
    sash.InsertHorizontalMullion(mullionSashNrArt, m2.Mullion.InsertionPointY)
    frame.InsertGlasspackets(glasspacket)
    if ctx.InsertFittings:
        args = sash.GetFittingsFindArgs()
        fittingsTypeID = db.FindFittingsType(fittingsGroup, args)
        sash.InsertFittings(fittingsTypeID, True)";
        }

        private void fksls_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtCode.Text = @"if pos.IsConstruction:
    area = pos.GetEmptyArea()
    if area.Width < 600:
        msg.Fail('Oblast pro okno je příliš úzká.')
    if area.Height < 1000:
        msg.Fail('Oblast pro okno je příliš nízká.')

    typeID = ctx.WindowsType
    colorID = ctx.WindowsColor
    glasspacket = ctx.Glasspacket
    type = db.GetWindowType(typeID)
    fittingsGroup = ctx.FittingsGroup
    mullionFrameVerNrArt = type.Mullions.Vertical.Frame.Default
    mullionFrameHorNrArt = type.Mullions.Horizontal.Frame.Default
    mullionSashNrArt = type.Mullions.Vertical.Sash.Default

    frame = area.InsertFrame(typeID, colorID)
    m1 = frame.InsertHorizontalMullion(mullionFrameVerNrArt)
    sash = m1.Area2.InsertSash()
    m2 = m1.Area1.InsertVerticalMullion(mullionFrameHorNrArt)
    sash.InsertVerticalMullion(mullionSashNrArt, m2.Mullion.InsertionPointX)
    frame.InsertGlasspackets(glasspacket)
    if ctx.InsertFittings:
        args = sash.GetFittingsFindArgs()
        args.TiltOnly = True
        fittingsTypeID = db.FindFittingsType(fittingsGroup, args)
        sash.InsertFittings(fittingsTypeID)";
        }

        private void _2xohr_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtCode.Text = @"if pos.IsConstruction:
    area = pos.GetEmptyArea()
    r = area.InsertVerticalCoupling('Ohraničník', 0)
    r.Area2.InsertHorizontalCoupling('Ohraničník', 0, 0.666)";
        }

        private void _3xRoz_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            txtCode.Text = @"if pos.IsConstruction:
    colorID = ctx.WindowsColor
    prof = '14631'
    area = pos.GetEmptyArea()
    r1 = area.InsertHorizontalCoupling(prof, colorID)
    r1.Coupling.SlideToTop()
    r2 = r1.Area2.InsertVerticalCoupling(prof, colorID)
    r2.Coupling.SlideToLeft()
    r3 = r2.Area2.InsertVerticalCoupling(prof, colorID)
    r3.Coupling.SlideToRight()";
        }
    }
}
