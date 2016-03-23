using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Okna.Plugins;

namespace EOkno.ViewModels
{
    public class PositionViewModel : DocumentViewModel
    {
        private const string s_Inherit = "doc";
        private const string s_True = "1";
        private const string s_False = "0";

        private XElement _data;
        private bool _created;
        private List<XElement> _removedContent;

        private bool _inheritFromDocument;
        public bool InheritFromDocument
        {
            get { return _inheritFromDocument; }
            set
            {
                if (_inheritFromDocument != value)
                {
                    _inheritFromDocument = value;
                    OnPropertyChanged(nameof(InheritFromDocument));

                    if (_data != null)
                    {
                        var attr = _data.Attribute(s_Inherit);
                        if (attr != null)
                        {
                            string properValue = (_inheritFromDocument) ? s_True : s_False;
                            if (attr.Value == properValue)
                            {
                                return;
                            }
                        }

                        _data.SetAttributeValue(s_Inherit, (_inheritFromDocument) ? s_True : s_False);
                        if (_inheritFromDocument)
                        {
                            // odstranit všechen obsah
                            _removedContent = _data.Elements().ToList();
                            _data.RemoveNodes();
                        }
                        else
                        {
                            // nechat zaregistrovat obsah
                            if (_removedContent != null && _removedContent.Count != 0)
                            {
                                // konfigurace podle uloženého nastavení
                                _data.Add(_removedContent);
                                base.SetMainElement(_data, false);
                            }
                            else
                            {
                                // výchozí stav podle volání z View
                                base.SetMainElement(_data, _created);
                            }
                        }
                    }
                }
            }
        }

        internal override void SetMainElement(XElement data, bool created)
        {
            _data = data;
            _created = created;

            if (created)
            {
                this.InheritFromDocument = true;
            }
            else
            {
                this.InheritFromDocument = data.GetAttrValue(s_Inherit, s_True) == s_True;
                if (!this.InheritFromDocument)
                {
                    base.SetMainElement(data, created);
                }
            }
        }
    }
}
