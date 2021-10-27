using Common.Commentaries.Model;
using Common.Commentaries.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace OSISTools.Commentaries.Tools
{
    public class OSISCommentariesSaver : ICommentariesSaver
    {
        private readonly OSISFormater _formater;

        public OSISCommentariesSaver(OSISFormater formater)
        {
            _formater = formater;
        }

        #region Methods

        private void WriteHeader(XmlWriter writer)
        {
            writer.WriteStartElement("header");

            writer.WriteElementString("osisIDWork", "FrPAS");
            writer.WriteElementString("osisRefWork", "Commentary");
            writer.WriteElementString("lang", "xml", "fr");
            writer.WriteElementString("canonical", "true");

            writer.WriteStartElement("work");
            writer.WriteElementString("osisWork", "FrPAS");
            writer.WriteStartElement("title");
            writer.WriteString("Plaire au Seigneur");
            writer.WriteEndElement();

            writer.WriteStartElement("identifier");
            writer.WriteElementString("type", "OSIS");
            writer.WriteString("FrPAS");
            writer.WriteEndElement();

            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        public void Save(IEnumerable<Commentaire> commentaires, string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            settings.CloseOutput = true;
            settings.OmitXmlDeclaration = true;
            using XmlWriter writer = XmlWriter.Create($"{filename}.osis.xml", settings);

            writer.WriteStartElement("osis", "http://www.bibletechnologies.net/2003/OSIS/namespace");

            writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns", "osis", null, "http://www.bibletechnologies.net/2003/OSIS/namespace");
            writer.WriteAttributeString("xsi", "schemaLocation", null, "http://www.bibletechnologies.net/2003/OSIS/namespace http://www.bibletechnologies.net/osisCore.2.1.1.xsd");

            writer.WriteStartElement("osisText");

            WriteHeader(writer);

            OSISReferenceConverter _referenceConverter = new OSISReferenceConverter();

            writer.WriteStartElement("div");
            writer.WriteAttributeString("type", "bookGroup");

            foreach (Commentaire commentaire in commentaires)
            {

                List<Reference> referencesList = _referenceConverter.ConvertReference(commentaire.Reference);


                writer.WriteStartElement("div");
                writer.WriteAttributeString("type", "section");
                writer.WriteAttributeString("annotateType", "commentary");
                writer.WriteAttributeString("annotateRef", _referenceConverter.ReferenceToBookLink(referencesList.First()));

                writer.WriteRaw(_formater.ToString(commentaire));

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
        }

        #endregion Methods
    }
}