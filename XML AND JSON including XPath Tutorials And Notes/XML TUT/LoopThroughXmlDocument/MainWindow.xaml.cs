using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

//Common XML DOM(DOCUMENT OBJECT MODEL) Classes
//XmlNode - Represents a single node in a document tree. If this node represents the root of an XML document, you can navigate to
//any position in the document from it
//XmlDocument - Extends the XmlNode class, used to load and save data from disk or elsewhere
//XmlElement - Reps a single element in the XML document.Derieved from XmlLinkedNode which in turn derieved from XmlNode
//XmlAttribute - Reps a single attribute. Derieved from XmlNode
//XmlText - Reps the text between a starting tag and closing tag
//XmlComment - Reps a special kind of node that is not regarded as part of the document other than to provide info to the reader about
//parts of the document
//XmlNodeList - Reps a collection of nodes

//XmlElement element that comes from XmlDocument.DocumentElement represents the root element of the XmlDocument => gives acces to evry 
//bit of info in the document
//XmlElement Properties:
//FirstChild => Returns the first child element after this once as XmlNode object, 
//test type of returned node b/c can be XmlElement or something else like
//XmlText => child of Title elemtn is an XmlText node that reps the text Beginning Visual C#
//LastChild =>Returns last child of the current node
//ParentNode => Returns the parent of the current node
//NextSibiling => Returns the next node that has the same parent node. 
//HasChildNodes => Check whether the current element has child elements w/o actually getting the value from FirstChild and examining null

namespace LoopThroughXmlDocument
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void buttonLoop_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load(@"C:\Users\HS1122\Desktop\C#\XML TUT\XMLtut.xml");
            textBlockResults.Text = FormatText(document.DocumentElement as XmlNode, "", "");
        }

        private string FormatText(XmlNode node, string text, string indent)
        {

            //Nodes of the type XmlElement return null if you use their Value property. But it is possible to get the
            //info between the starting and closing tags of an XmlElement if you use one of two other methodes,
            //InnerText and InnerXml.
            //InnerText => Gets the text of all the child nodes of the current node and returns it as a single concatenated string 
            //Can change the text using this method
            //InnerXml => returns the text like InnerText but it aslso returns all of the tags. 
            //=> As you can see this can be quite useful if you have a string containing XML that you want to inject directly into
            //your XML document. 
            //Value => The cleanest way to maipulate information in the document. The classes that return values are:
            //XmlText, XmlComment, XmlAttribute

            //You can create nodes in doc using
            //CreateNode => Creates any kind of node => Three overloads of the method, 2 of which enable you to create nodes of the
                            //type found in the XmlNodeType enumenration and one that enables you to specify the type of the node
                            //to use as a string.The method returns an instance of XmlNode that can then be cast to the approp
                            //type explicitely
            //CreateElement => Version of CreateNode that creates only nodes of the XmlElement variety
            //CreateAttribute => CreateNode that creates only nodes of the XmlAttribute variety
            //CreateTextNode => Creates XmlTextNode
            //CreateComment => Comment
            //Immed after creating node, add info, and insert into doc using these methods

            //AppendChild => Appends a child node to a node of type XmlNode. Appears at the bottom of the list of children.
            //InsertAfter => Controls where exactly you want to insert, keeps two paramters, the first is the new node, second is the
                            //node after which the new node should be inserted
            //InsertBefore

            //RemoveAll => Removes all child nodes in the node on which it is called. What is slightly less obvious is that it removes
                           //all attributes on the node because they are regarded as childs nodes as well.
            //RemoveChild => REMOVES A SINGLE child in the node on which it is called. The method returns the node that has been 
                           //removed from the doc.

            //SelectSingleNode => Selects a single node. If you create a query that fetches more than one node, only the first node 
            //                    will be returned
            //SelectNodes => Returns a node collection in the form of an XmlNodeList class

                if (node is XmlText) //is keyword allows you to check type during runtime
            {
                text += node.Value;
                return text;
            }

            if (string.IsNullOrEmpty(indent))
                indent = "";
            else
            {
                text += "\r\n" + indent;
            }

            if (node is XmlComment)
            {
                text += node.OuterXml;
                return text;
            }

            text += "<" + node.Name;
            if (node.Attributes.Count > 0)
            {
                AddAttributes(node, ref text);
            }
            if (node.HasChildNodes)
            {
                text += ">";
                foreach (XmlNode child in node.ChildNodes)
                {
                    text = FormatText(child, text, indent + "  ");
                }
                if (node.ChildNodes.Count == 1 &&
                   (node.FirstChild is XmlText || node.FirstChild is XmlComment))
                    text += "</" + node.Name + ">";
                else
                    text += "\r\n" + indent + "</" + node.Name + ">";
            }
            else
                text += " />";
            return text;
        }

        private void AddAttributes(XmlNode node, ref string text)
        {
            foreach (XmlAttribute xa in node.Attributes)
            {
                text += " " + xa.Name + "='" + xa.Value + "'";
            }
        }


        
                private void buttonCreateNode_Click(object sender, RoutedEventArgs e)
                {
                    // Load the XML document.
                    XmlDocument document = new XmlDocument();
                    document.Load(@"C:\Users\HS1122\Desktop\C#\XML TUT\XMLFile1.xml");

                    // Get the root element.
                    XmlElement root = document.DocumentElement;

                    // Create the new nodes.
                    XmlElement newBook = document.CreateElement("book");
                    XmlElement newTitle = document.CreateElement("title");
                    XmlElement newAuthor = document.CreateElement("author");
                    XmlElement newCode = document.CreateElement("code");
                    XmlText title = document.CreateTextNode("Beginning Visual C# 2012");
                    XmlText author = document.CreateTextNode("Karli Watson et al");
                    XmlText code = document.CreateTextNode("314418");
                    XmlComment comment = document.CreateComment("The previous edition");
                    XmlAttribute HELLO = document.CreateAttribute("pages");
                    HELLO.Value = "1000";

                    // Insert the elements.
                    newBook.AppendChild(comment);
                    newBook.AppendChild(newTitle);
                    newBook.AppendChild(newAuthor);
                    newBook.AppendChild(newCode);
                    newTitle.AppendChild(title);
                    newAuthor.AppendChild(author);
                    newCode.AppendChild(code);
                    newAuthor.Attributes.Append(HELLO);
                    root.InsertAfter(newBook, root.LastChild);

                    document.Save(@"C:\Users\HS1122\Desktop\C#\XML TUT\XMLFile1.xml");
                }

                private void buttonDeleteNode_Click_1(object sender, RoutedEventArgs e)
                {
                    // Load the XML document.
                    XmlDocument document = new XmlDocument();
                    document.Load(@"C:\Users\HS1122\Desktop\C#\XML TUT\XMLFile1.xml");

                    // Get the root element.
                    XmlElement root = document.DocumentElement;

                    // Find the node. root is the <books> tag, so its last child
                    // which will be the last <book> node.
                    if (root.HasChildNodes)
                    {
                        XmlNode book = root.LastChild;

                        // Delete the child.
                        root.RemoveChild(book);

                        // Save the document back to disk.
                        document.Save(@"C:\Users\HS1122\Desktop\C#\XML TUT\XMLFile1.xml");
                    }
                }

 
           private void buttonXMLtoJSON_Click(object sender, RoutedEventArgs e)
  {
      // Load the XML document.
      XmlDocument document = new XmlDocument();

      document.Load(@"C:\Users\HS1122\Desktop\C#\XML TUT\XMLtut.xml");

      string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(document);

      textBlockResults.Text = json;

    }
    }
}
