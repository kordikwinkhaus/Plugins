using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using UserExtensions;

namespace Union
{
    public partial class VolbyZakazky : UserControl, IUserForm, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private const string s_MainElement = "Union";

        private const string s_OdvodneniRamu = "OdvodneniRamu";
        private const string s_Dopredu = "Dopredu";
        private const string s_Dolu = "Dolu";
        private const string s_BezOdvodneni = "BezOdvodneni";
        private const string s_VychoziOdvodneni = s_Dopredu;

        private const string s_OdvodneniRamuVen = "OdvodneniRamuVen";
        private const string s_ZasklList = "ZasklList";
        private const string s_BezOdvodneniVen = "BezOdvodneni";
        private const string s_VychoziOdvodneniVen = s_ZasklList;


        public VolbyZakazky()
        {
            InitializeComponent();
        }

        #region Odvodnění dovnitř

        private string Odvodneni
        {
            get
            {
                if (ObjectData != null && ObjectData.Element(s_OdvodneniRamu) != null)
                {
                    return ObjectData.Element(s_OdvodneniRamu).Value;
                }

                return s_VychoziOdvodneni;
            }
            set
            {
                if (ObjectData == null) return;

                try
                {
                    NotifyPropertyChanging("Odvodneni");

                    // vždy element odebrat
                    if (ObjectData.Element(s_OdvodneniRamu) != null)
                    {
                        ObjectData.Element(s_OdvodneniRamu).Remove();
                    }

                    // a pokud to není výchozí způsob, znovu vytvořit
                    if (value != s_VychoziOdvodneni)
                    {
                        ObjectData.SetElementValue(s_OdvodneniRamu, value);
                    }

                    NotifyPropertyChanged("Odvodneni");
                    NotifyPropertyChanged("BezOdvodneni");
                    NotifyPropertyChanged("OdvodneniDopredu");
                    NotifyPropertyChanged("OdvodneniDolu");
                    NotifyPropertyChanged("OdvodneniZasklList");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetType().ToString() + ": " + ex.Message);
                }
            }
        }

        public bool BezOdvodneni
        {
            get { return this.Odvodneni == s_BezOdvodneni; }
            set
            {
                if (value)
                {
                    this.Odvodneni = s_BezOdvodneni;
                }
            }
        }

        public bool OdvodneniDopredu
        {
            get { return this.Odvodneni == s_Dopredu; }
            set
            {
                if (value)
                {
                    this.Odvodneni = s_Dopredu;
                }
            }
        }

        public bool OdvodneniDolu
        {
            get { return this.Odvodneni == s_Dolu; }
            set
            {
                if (value)
                {
                    this.Odvodneni = s_Dolu;
                }
            }
        }

        #endregion

        #region Odvodnění ven

        private string OdvodneniVen
        {
            get
            {
                if (ObjectData != null && ObjectData.Element(s_OdvodneniRamuVen) != null)
                {
                    return ObjectData.Element(s_OdvodneniRamuVen).Value;
                }

                return s_VychoziOdvodneniVen;
            }
            set
            {
                if (ObjectData == null) return;

                try
                {
                    NotifyPropertyChanging("OdvodneniVen");

                    // vždy element odebrat
                    if (ObjectData.Element(s_OdvodneniRamuVen) != null)
                    {
                        ObjectData.Element(s_OdvodneniRamuVen).Remove();
                    }

                    // a pokud to není výchozí způsob, znovu vytvořit
                    if (value != s_VychoziOdvodneniVen)
                    {
                        ObjectData.SetElementValue(s_OdvodneniRamuVen, value);
                    }

                    NotifyPropertyChanged("BezOdvodneniVen");
                    NotifyPropertyChanged("OdvodneniZasklList");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.GetType().ToString() + ": " + ex.Message);
                }
            }
        }

        public bool BezOdvodneniVen
        {
            get { return this.OdvodneniVen == s_BezOdvodneniVen; }
            set
            {
                if (value)
                {
                    this.OdvodneniVen = s_BezOdvodneniVen;
                }
            }
        }

        public bool OdvodneniZasklList
        {
            get { return this.OdvodneniVen == s_ZasklList; }
            set
            {
                if (value)
                {
                    this.OdvodneniVen = s_ZasklList;
                }
            }
        }

        #endregion

        private XElement _objectData;
        public XElement ObjectData
        {
            get { return _objectData; }
            set
            {
                NotifyPropertyChanging("ObjectData");
                _objectData = value;
                _objectData.Changed += new EventHandler<XObjectChangeEventArgs>(XmlChanged);
                NotifyPropertyChanged("ObjectData");
            }
        }

        private void XmlChanged(object sender, XObjectChangeEventArgs e)
        {
            NotifyPropertyChanged("Odvodneni");
        }

        public bool SetData(XElement data, int document, int position, int profileType)
        {
            if (data == null) return false;

            if (data.Element(s_MainElement) == null)
            {
                data.SetElementValue(s_MainElement, string.Empty);
            }

            ObjectData = data.Element(s_MainElement);
            DataContext = this;
            XmlChanged(this, null);

            return true;
        }

        public object Title
        {
            get { return "Rozšířené volby"; }
        }

        public event PropertyChangingEventHandler PropertyChanging;
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
