using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WHOkna;

namespace EOkno.Tests
{
    internal class FakeOknaDocument : IOknaDocument
    {
        public IPosition ActivePos
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IOknaApplication Application
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Client
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public XElement ClientData
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime CompletionDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int ContextID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DateTime ContractDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Currency
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Dealer
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int DeliveryAddress
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public XElement DeliveryData
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime DeliveryDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool DoCheckPoints
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public XElement ExtendedProperties { get; set; }

        public int Index
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public DateTime InstallationDate
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Mask
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Modified
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal MontageValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Note
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public EOptState OptState
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public decimal PartsValue
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int PosCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IPosition> Positions
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool ReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Reference
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Source
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string State
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Status
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Undo
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool UpdateTechnology
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public XElement UserTechChanges
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool Verbose
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public List<string> Warrnings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event DocumentNotification AfterSave;
        public event DocumentNotification BeforeSave;
        public event DocumentNotification Changed;
        public event SelectionNotification SelectionChanged;

        public int ActivateContext()
        {
            throw new NotImplementedException();
        }

        public bool AddArtForOpt(IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public IPosition AddPos()
        {
            throw new NotImplementedException();
        }

        public bool ChangeTotal(float stawka, int Column, ValueType new_tot, int kategorie, int podkategorie)
        {
            throw new NotImplementedException();
        }

        public void CleanUpForReuse()
        {
            throw new NotImplementedException();
        }

        public void CreateCheckPoint(IPart part)
        {
            throw new NotImplementedException();
        }

        public void DeactivateContext()
        {
            throw new NotImplementedException();
        }

        public IPart FromGUID(uint guid)
        {
            throw new NotImplementedException();
        }

        public IPart FromGUID(string guid)
        {
            throw new NotImplementedException();
        }

        public IPart FromGUID(uint guid, int context)
        {
            throw new NotImplementedException();
        }

        public IPart FromGUID(string guid, int context)
        {
            throw new NotImplementedException();
        }

        public object GetClassicAutomationInterface()
        {
            throw new NotImplementedException();
        }

        public IPosition GetPos(int pos)
        {
            throw new NotImplementedException();
        }

        public int GetSubDocIndex(string guid)
        {
            throw new NotImplementedException();
        }

        public int GetSubDocIndex(IPosition pos)
        {
            throw new NotImplementedException();
        }

        public IPosition InsertPos(int at)
        {
            throw new NotImplementedException();
        }

        public bool LoadFromBytes(byte[] data)
        {
            throw new NotImplementedException();
        }

        public IPricelistData LoadPricelistData(int index, int color)
        {
            throw new NotImplementedException();
        }

        public uint OriginalGUID(uint guid)
        {
            throw new NotImplementedException();
        }

        public bool PerformUndo()
        {
            throw new NotImplementedException();
        }

        public void RemoveCheckPoint()
        {
            throw new NotImplementedException();
        }

        public bool RemovePos(int pos)
        {
            throw new NotImplementedException();
        }

        public uint Report()
        {
            throw new NotImplementedException();
        }

        public XDocument ReportXML()
        {
            throw new NotImplementedException();
        }

        public void ResetWarrnings()
        {
            throw new NotImplementedException();
        }

        public bool RunMachineScript(ushort line, string directory, string filename, string ext)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Save(string mask)
        {
            throw new NotImplementedException();
        }

        public byte[] SaveAsBytes()
        {
            throw new NotImplementedException();
        }

        public byte[] SaveAsBytes(string version)
        {
            throw new NotImplementedException();
        }

        public bool SaveAsVerbose()
        {
            throw new NotImplementedException();
        }

        public int TranslateColor(int idx_koloru_prof, EColorType typ)
        {
            throw new NotImplementedException();
        }

        public void UpdateViews(int hint)
        {
            throw new NotImplementedException();
        }
    }
}
