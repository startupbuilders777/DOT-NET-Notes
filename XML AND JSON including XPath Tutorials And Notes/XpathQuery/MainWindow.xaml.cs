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

namespace XpathQuery
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private XmlDocument document;

    public MainWindow()
    {
      //Xpath query
      //If nothing else is stated, the XPath query example makes a selection that is relative to the node on which it is performed.
      //Where it is necessary to hava a node name, you can assume the current node is the <element> node in the XML document

            //  .  => Selects the current node
            //  .. => Select the parent of the current node
            // * => select all nodes of the current node
            // Title => Select all child nodes with a specific name, in this case, title.
            // @Type => Select an attribute of the current node
            // @* => Select all attributes of the current node
            // element[2] => Select a child node by index, in this case, the second element node
            // text() => Select all the text nodes of the current node
            // element/text() => Select one or more grandchildren of the current node
            // //mass => Select all nodes in the document with a particular name - in this case, all mass nodes.
            //  //element/name = > Select all nodes in the doc with a particular name and a particular parent name, the parent name is
                                   //element and the node name is name. 
            //   //element[name = 'Hydrogen'] => Select a node where a value criterion is met -> in this case, the elemnt for which 
                                                //the name of the element is Hydrogen
            //   //elemnt[@Type ='Nobel Gas']  => Select a node where an attribute criterion is met, int his case, the Type attribute
                                                 //is Noble Gase.
                                                 
            /*
             Heres some stuff you can type in to query. Guess what it queries!!!
             //elements
             element
             element[@Type = 'Nobel Gas']
             //mass
             //mass/..
             element/specification[mass = '20.1797']
             element/name[text() = 'Neon']



             */ 


      InitializeComponent();

      document = new XmlDocument();
      document.Load(@"C:\Users\HS1122\Desktop\C#\XpathQuery\Elements.xml");
      Update(document.DocumentElement.SelectNodes("."));
    }

    private void Update(XmlNodeList nodes)
    {
      if (nodes == null || nodes.Count == 0)
      {
        textBlockResult.Text = "The query yielded no results";
        return;
      }
      string text = "";
      foreach (XmlNode node in nodes)
      {
        text = FormatText(node, text, "") + "\r\n";
      }
      textBlockResult.Text = text;
    }

        private string FormatText(XmlNode node, string text, string indent)
    {
      if (node is XmlText)
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

    private void buttonExecute_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        XmlNodeList nodes = document.DocumentElement.SelectNodes(textBoxQuery.Text);
        Update(nodes);
      }
      catch (Exception err)
      {
        textBlockResult.Text = err.Message;
      }
    }
  }

}
