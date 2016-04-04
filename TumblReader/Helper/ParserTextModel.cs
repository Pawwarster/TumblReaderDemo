using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TumblReader.Helper
{
    /// <summary>
    /// Class for ParserTextModel, parses HTML into controls
    /// </summary>
    public class ParserTextModel
    {
        #region Variables
        //-------------------Variables--------------------//

        //-------------------Settings---------------------//

        private string brCharReplace = "\n";
        private char liCharLeftPad2 = '○';
        private char liCharLeftPad3 = '□';
        private char liCharLeftPad = '●';
        private int liAdditionalSpaceNumber = 3;
        private Thickness imageControlMargin = new Thickness(0, 5, 0, 5);
        private int horizontalLineHeight = 1;
        // private  Color horizontalLineColor = Color.FromArgb(255, 19, 70, 113);
        private Color horizontalLineColor = Colors.LightGray;
        private Color hBackgroundColor = Color.FromArgb(255, 255, 255, 255);
        private Color hForegroundColor = Color.FromArgb(255, 0, 0, 0);
        private HorizontalAlignment tableAlignment = HorizontalAlignment.Left;
        private FontWeight textFontWeight = FontWeights.Thin;
        private TextAlignment textAlignment = TextAlignment.Left;
        private FontStyle textFontStyle = FontStyles.Normal;
        private int maximumTextCount = 600;
        private bool parseOnlyFirstListGeration;
        private bool addLineAfterLi;
        private ICommand hrefCommand;
        private bool dontAddCharLeftPad;
        private bool linkDontUserTextureDecoration;
        private bool dontParseTable;

        private Color textColor = Color.FromArgb(255, 60, 66, 71);
        private Color aHrefColor = Color.FromArgb(255, 0, 0, 255);
        private Color aHrefCheckedColor = Color.FromArgb(255, 0, 75, 168);

        private int textFontSize = 22;
        private int h1TextFontSize = 30;
        private int h2TextFontSize = 23;
        private int h3TextFontSize = 26;
        private int h4TextFontSize = 25;
        private int h5TextFontSize = 24;

        private static int liFontSize = 22;

        //-------------------Temp-Variables---------------------//
        private int liGenerationCounter;
        private List<UIElement> resultListControl;
        private RichTextBox currentRichTextBox;
        private Paragraph currentParagraph;
        private int additionalSpace;

        private HtmlDocument htmlDocument;
        private string htmlToParse;
        private bool firstTableParse;

        /// <summary>
        /// Stores the TextAlignment information
        /// </summary>
        public TextAlignment TextAlignment
        {
            get
            {
                return textAlignment;
            }
            set
            {
                textAlignment = value;
            }
        }
        /// <summary>
        /// Flag indicating if the is no need to add char's to the left od the lists
        /// </summary>
        public bool DontAddCharLeftPad
        {
            get
            {
                return dontAddCharLeftPad;
            }
            set
            {
                dontAddCharLeftPad = value;
            }
        }
        /// <summary>
        /// Flag indicating if the is no need to parse table
        /// </summary>
        public bool DontParseTable
        {
            get
            {
                return dontParseTable;
            }
            set
            {
                dontParseTable = value;
            }
        }
        /// <summary>
        /// Flag indicating if there is no need to decorate links
        /// </summary>
        public bool LinkDontUserTextureDecoration
        {
            get
            {
                return linkDontUserTextureDecoration;
            }
            set
            {
                linkDontUserTextureDecoration = value;
            }
        }
        /// <summary>
        /// Hanlder for defining command for href's in HTML
        /// </summary>
        public ICommand HrefCommand
        {
            get
            {
                return hrefCommand;
            }
            set
            {
                hrefCommand = value;
            }
        }
        /// <summary>
        /// Flag indicating if there is a need to add separation line between list objects
        /// </summary>
        public bool AddLineAfterLi
        {
            get
            {
                return addLineAfterLi;
            }
            set
            {
                addLineAfterLi = value;
            }
        }
        /// <summary>
        /// Flag indicating if there is need to parse only first list
        /// </summary>
        public bool ParseOnlyFirstListGeration
        {
            get
            {
                return parseOnlyFirstListGeration;
            }
            set
            {
                parseOnlyFirstListGeration = value;
            }
        }
        /// <summary>
        /// Stores foreground color of the 'h' HTML element
        /// </summary>
        public Color HForegroundColor
        {
            get
            {
                return hForegroundColor;
            }
            set
            {
                hForegroundColor = value;
            }
        }
        /// <summary>
        /// Stores background color of the 'h' HTML element
        /// </summary>
        public Color HBackgroundColor
        {
            get
            {
                return hBackgroundColor;
            }
            set
            {
                hBackgroundColor = value;
            }
        }
        /// <summary>
        /// Stores the maximum characters count in text
        /// </summary>
        public int MaximumTextCount
        {
            get
            {
                return maximumTextCount;
            }
            set
            {
                maximumTextCount = value;
            }
        }
        /// <summary>
        /// Stores FontStyle for the displayed text
        /// </summary>
        public FontStyle TextFontStyle
        {
            get
            {
                return textFontStyle;
            }
            set
            {
                textFontStyle = value;
            }
        }
        /// <summary>
        /// Stores FontWeight for displayed text (ex. bold, light)
        /// </summary>
        public FontWeight TextFontWeight
        {
            get
            {
                return textFontWeight;
            }
            set
            {
                textFontWeight = value;
            }
        }
        /// <summary>
        /// Stores HorizontalAlignment information for table element
        /// </summary>
        public HorizontalAlignment TableAlignment
        {
            get
            {
                return tableAlignment;
            }
            set
            {
                tableAlignment = value;
            }
        }
        /// <summary>
        /// Store Height value for horizontal lines
        /// </summary>
        public int HorizontalLineHeight
        {
            get
            {
                return horizontalLineHeight;
            }
            set
            {
                horizontalLineHeight = value;
            }
        }
        /// <summary>
        /// Stores Color for the horizontal lines
        /// </summary>
        public Color HorizontalLineColor
        {
            get
            {
                return horizontalLineColor;
            }
            set
            {
                horizontalLineColor = value;
            }
        }
        /// <summary>
        /// Stores margin information for Image controls
        /// </summary>
        public Thickness ImageControlMargin
        {
            get
            {
                return imageControlMargin;
            }
            set
            {
                imageControlMargin = value;
            }
        }
        /// <summary>
        /// List storing completed list of parsed controls
        /// </summary>
        public List<UIElement> ResultListControl
        {
            get
            {
                return resultListControl;
            }
        }
        /// <summary>
        /// Stores color for the text to display
        /// </summary>
        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
            }
        }
        /// <summary>
        /// Stores color for the link in text
        /// </summary>
        public Color AHrefColor
        {
            get
            {
                return aHrefColor;
            }
            set
            {
                aHrefColor = value;
            }
        }
        /// <summary>
        /// Stores color for the clicked link in text
        /// </summary>
        public Color AHrefCheckedColor
        {
            get
            {
                return aHrefCheckedColor;
            }
            set
            {
                aHrefCheckedColor = value;
            }
        }
        /// <summary>
        /// Stores size of the font for text
        /// </summary>
        public int TextFontSize
        {
            get
            {
                return textFontSize;
            }
            set
            {
                textFontSize = value;
            }
        }
        /// <summary>
        /// Stores font size for list elements
        /// </summary>
        public static int LiFontSize
        {
            get
            {
                return liFontSize;
            }
            set
            {
                liFontSize = value;
            }
        }
        /// <summary>
        /// Stores HTML that needs parsing to controls
        /// </summary>
        public string HtmlToParse
        {
            get
            {
                return htmlToParse;
            }
        }
        /// <summary>
        /// Stores char value for replacing 'br' HTML elements
        /// </summary>
        public string BrCharReplace
        {
            get
            {
                return brCharReplace;
            }
            set
            {
                brCharReplace = value;
            }
        }
        /// <summary>
        /// Stores char for dotting the list elements
        /// </summary>
        public char LiCharLeftPad
        {
            get
            {
                return liCharLeftPad;
            }
            set
            {
                liCharLeftPad = value;
            }
        }
        /// <summary>
        /// Stores second char for dotting the list elements
        /// </summary>
        public char LiCharLeftPad2
        {
            get
            {
                return liCharLeftPad2;
            }
            set
            {
                liCharLeftPad2 = value;
            }
        }
        /// <summary>
        /// Stores third char for dotting the list elements
        /// </summary>
        public char LiCharLeftPad3
        {
            get
            {
                return liCharLeftPad3;
            }
            set
            {
                liCharLeftPad3 = value;
            }
        }
        /// <summary>
        /// Stores additional number of spaces between list elements
        /// </summary>
        private int LiAdditionalSpaceNumber
        {
            get
            {
                return liAdditionalSpaceNumber;
            }
            set
            {
                liAdditionalSpaceNumber = value;
            }
        }

        #endregion
        /// <summary>
        /// Method for parsing provided HTML into controls
        /// </summary>
        /// <param name="stringHtmlToParse">HTML to parse</param>
        /// <returns>List of parsed controls</returns>
        public List<UIElement> Parse(string stringHtmlToParse)
        {
            try
            {
                this.htmlToParse = stringHtmlToParse.Replace("<br />", brCharReplace).Replace("<br>", brCharReplace).Replace("<br/>", brCharReplace);
                LinkParse();

                additionalSpace = 0;

                htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlToParse);


                CreateRichTextBox();
                resultListControl = new List<UIElement>();

                ParseTags(htmlDocument.DocumentNode);
            }
            catch { }

            return resultListControl;
        }
        /// <summary>
        /// Method for parsing links ('a' HTML elements)
        /// </summary>
        public void LinkParse()
        {
            int indexStart = 0;
            int indexEnd = 0;
            int startIndex = 0;

            while ((indexStart = htmlToParse.IndexOf("<link", startIndex)) != -1)
            {
                try
                {
                    indexEnd = htmlToParse.IndexOf('>', indexStart);
                    string tempString = htmlToParse.Substring(indexStart, indexEnd - indexStart + 1);
                    string[] ar = tempString.Split(' ');
                    string pathEnd = "<a href=\"";
                    if (ar[1].Replace("http", "").Length < ar[1].Length)
                    {
                        pathEnd += ar[1];
                        if (pathEnd[pathEnd.Length - 1] == '>')
                        {
                            pathEnd = pathEnd.Remove(pathEnd.Length - 1, 1);
                        }
                    }
                    else
                    {
                        if (pathEnd[pathEnd.Length - 2] == '>')
                        {
                            pathEnd = pathEnd.Remove(pathEnd.Length - 2, 1);
                        }

                    }

                    //ar.Length < 3 && 
                    pathEnd += "\">";
                    htmlToParse = htmlToParse.Remove(indexStart, indexEnd - indexStart + 1).Insert(indexStart, pathEnd);
                    startIndex = indexStart + 1;
                    if (startIndex > htmlToParse.Length - 1)
                        startIndex = htmlToParse.Length - 1;
                }
                catch
                {
                    startIndex = indexStart + 1;
                }
                // string temp = string.Empty;
            }
            htmlToParse = htmlToParse.Replace("</link>", "</a>");
        }
        /// <summary>
        /// Method for creating RichTextBox control storing the variety of text controls
        /// </summary>
        private void CreateRichTextBox()
        {
            currentParagraph = new Paragraph();
            currentRichTextBox = new RichTextBox();
            currentRichTextBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            currentRichTextBox.TextWrapping = TextWrapping.Wrap;
            currentRichTextBox.TextAlignment = textAlignment;

            if (additionalSpace > 0)
            {
                currentRichTextBox.Padding = new Thickness(additionalSpace * 10, 0, 0, 0);
            }
        }
        /// <summary>
        /// Method for parsing specific HTML node, based on it's tag
        /// </summary>
        /// <param name="rootNode">Frgament of HTML</param>
        private void ParseTags(HtmlNode rootNode)
        {
            foreach (HtmlNode tempNode in rootNode.ChildNodes)
            {
                switch (tempNode.Name)
                {
                    case "p":
                        ParseP(tempNode);
                        break;
                    case "#text":
                        ParseText(tempNode);
                        break;
                    case "li":
                        ParseLi(tempNode);
                        break;
                    case "ul":
                        ParseUl(tempNode);
                        break;
                    case "h1":
                        ParseH1(tempNode);
                        break;
                    case "h2":
                        ParseH2(tempNode);
                        break;
                    case "h3":
                        ParseH3(tempNode);
                        break;
                    case "h4":
                        ParseH4(tempNode);
                        break;
                    case "h5":
                        ParseH5(tempNode);
                        break;
                    case "b":
                        ParseB(tempNode);
                        break;
                    case "i":
                        ParseI(tempNode);
                        break;
                    case "div":
                        ParseDiv(tempNode);
                        break;
                    case "img":
                        ParseImg(tempNode);
                        break;
                    case "hr":
                        ParseHr();
                        break;
                    case "span":
                        ParseSpan(tempNode);
                        break;
                    case "strong":
                        ParseStrong(tempNode);
                        break;
                    case "table":
                        ParseTable(tempNode);
                        break;
                    case "tbody":
                        ParseTbody(tempNode);
                        break;
                    case "tr":
                        ParseTr(tempNode);
                        break;
                    case "td":
                        ParseTd(tempNode);
                        break;
                    case "th":
                        ParseTh(tempNode);
                        break;
                    case "thead":
                        ParseThead(tempNode);
                        break;
                    case "font":
                        ParseFont(tempNode);
                        break;
                    default:
                        if (tempNode.Name != "#comment" && char.IsLetter(tempNode.Name[0]) &&
                            tempNode.Name != "script")
                        {
                            if (tempNode.HasChildNodes)
                                ParseTags(tempNode);
                            else ParseText(tempNode);
                        }
                        break;

                }
            }
        }
        /// <summary>
        /// Method for parsing font based on HTML fragment option
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseFont(HtmlNode htmlNode)
        {
            Color tempColor = textColor;
            try
            {
                foreach (HtmlAttribute attribute in htmlNode.Attributes)
                {
                    switch (attribute.Name.ToLower())
                    {
                        case "color":
                            if (attribute.Value[0] != '#')
                            {
                                ParseFontColor(attribute.Value);
                            }

                            break;
                    }
                }
            }
            catch
            {
            }
            ParseTags(htmlNode);
            textColor = tempColor;
        }
        /// <summary>
        /// Method for parsing color of the font
        /// </summary>
        /// <param name="nameColor">Name of the color</param>
        public void ParseFontColor(string nameColor)
        {
            switch (nameColor)
            {
                case "red":
                    textColor = Colors.Red;
                    break;
                case "lightred":
                    textColor = Colors.Magenta;
                    break;
                case "darkread":
                    textColor = Color.FromArgb(255, 139, 0, 0);
                    break;
                case "green":
                    textColor = Colors.Green;
                    break;
                case "lightgreen":
                    textColor = Color.FromArgb(255, 144, 238, 144);
                    break;
                case "darkgreen":
                    textColor = Color.FromArgb(255, 0, 139, 0);
                    break;
                case "blue":
                    textColor = Colors.Blue;
                    break;
                case "lightblue":
                    textColor = Colors.Cyan;
                    break;
                case "darkrblue":
                    textColor = Color.FromArgb(255, 0, 0, 139);
                    break;
                case "yellow":
                    textColor = Colors.Yellow;
                    break;
                case "lightyellow":
                    textColor = Color.FromArgb(255, 255, 255, 224);
                    break;
                case "darkryellow":
                    textColor = Color.FromArgb(255, 240, 230, 140);
                    break;

            }
        }
        /// <summary>
        /// Method for parsing font weight
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseThead(HtmlNode htmlNode)
        {
            FontWeight tempFontWeight = textFontWeight;
            textFontWeight = FontWeights.Normal;

            ParseTags(htmlNode);

            textFontWeight = tempFontWeight;
        }
        /// <summary>
        /// Method for parsing Table head
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseTh(HtmlNode htmlNode)
        {
            FontWeight tempFontWeight = textFontWeight;
            textFontWeight = FontWeights.Normal;
            if (currentRichTextBox != null &&
                currentRichTextBox.Blocks.Count == 0)
            {
                currentRichTextBox.FontWeight = textFontWeight;
            }

            ParseTags(htmlNode);

            textFontWeight = tempFontWeight;
        }
        /// <summary>
        /// Method for parsing table element
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseTd(HtmlNode htmlNode)
        {
            ParseTags(htmlNode);
            if (dontParseTable)
            {
                Rectangle rect = new Rectangle();
                rect.Margin = new Thickness(0, 15, 0, 0);
                rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                rect.Height = horizontalLineHeight;
                rect.Fill = new SolidColorBrush(horizontalLineColor);
                resultListControl.Add(rect);
            }
        }
        /// <summary>
        /// Method for parsing table row
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseTr(HtmlNode htmlNode)
        {
            currentParagraph.Inlines.Add(new LineBreak());

            ParseTags(htmlNode);
        }
        /// <summary>
        /// Method for parsing table body
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseTbody(HtmlNode htmlNode)
        {
            ParseTags(htmlNode);
        }
        /// <summary>
        /// Method for parsing whole table
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseTable(HtmlNode htmlNode)
        {
            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();

            if (!dontParseTable)
            {
                if (!(resultListControl[resultListControl.Count - 1] is Rectangle))
                {
                    Rectangle rect = new Rectangle();
                    rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                    rect.Height = horizontalLineHeight;
                    rect.Fill = new SolidColorBrush(horizontalLineColor);
                    resultListControl.Add(rect);
                }

                //ParseTags(htmlNode);

                HtmlNode htmlNodeThead = htmlNode.Element("thead");
                HtmlNode htmlNodeFoot = htmlNode.Element("tfoot");
                HtmlNode htmlNodeTBody = htmlNode.Element("tbody");


                if (htmlNodeThead != null)
                {
                    ParseTableElement(htmlNodeThead);
                }
                if (htmlNodeTBody != null)
                {
                    ParseTableElement(htmlNodeTBody);
                }
                else
                {
                    ParseTableElement(htmlNode);
                }
                if (htmlNodeFoot != null)
                {
                    ParseTableElement(htmlNodeFoot);
                }
            }
            else
            {
                if (!firstTableParse)
                {
                    Rectangle rect2 = new Rectangle();
                    rect2.HorizontalAlignment = HorizontalAlignment.Stretch;
                    rect2.Height = horizontalLineHeight;
                    rect2.Fill = new SolidColorBrush(horizontalLineColor);
                    rect2.Margin = new Thickness(0, 15, 0, 0);
                    resultListControl.Add(rect2);
                }
                firstTableParse = true;

                ParseTags(htmlNode);
            }
            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();
            if (!(resultListControl[resultListControl.Count - 1] is Rectangle))
            {
                Rectangle rect2 = new Rectangle();
                rect2.HorizontalAlignment = HorizontalAlignment.Stretch;
                rect2.Height = horizontalLineHeight;
                rect2.Fill = new SolidColorBrush(horizontalLineColor);

                resultListControl.Add(rect2);
            }
        }
        /// <summary>
        /// Method for parsing table element
        /// </summary>
        /// <param name="nodeTemp">HTML fragment</param>
        public void ParseTableElement(HtmlNode nodeTemp)
        {
            ParserTextModel parser = new ParserTextModel();
            parser.linkDontUserTextureDecoration = this.linkDontUserTextureDecoration;

            List<HtmlNode> htmlNodeTr = new List<HtmlNode>(nodeTemp.Elements("tr"));
            List<List<StackPanel>> listTr = new List<List<StackPanel>>();
            int maxCollumn = 0;

            for (int i = 0; i < htmlNodeTr.Count; ++i)
            {
                List<HtmlNode> htmlNodeTd = null;
                try
                {
                    htmlNodeTd = (from g in htmlNodeTr[i].ChildNodes where g.Name == "td" || g.Name == "th" select g).ToList<HtmlNode>();
                }
                catch
                {
                    htmlNodeTd = new List<HtmlNode>();
                }
                //new List<HtmlNode>(htmlNodeTr[i].Elements("td"));
                List<UIElement> controls = null;
                List<StackPanel> stackPanelList = new List<StackPanel>();

                if (htmlNodeTd.Count > 0)
                {
                    int textFontSizeTemp = textFontSize * (int)(1 / (float)htmlNodeTd.Count) + (int)(TextFontSize / 1.3f);

                    parser.TextFontSize = textFontSizeTemp;

                }

                for (int j = 0; j < htmlNodeTd.Count; ++j)
                {
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Vertical;
                    controls = parser.Parse("<p>" + htmlNodeTd[j].OuterHtml + "</p>");
                    foreach (UIElement block in controls)
                        stackPanel.Children.Add(block);
                    stackPanelList.Add(stackPanel);
                }

                listTr.Add(stackPanelList);
                if (htmlNodeTd.Count > maxCollumn)
                    maxCollumn = htmlNodeTd.Count;

            }

            Grid grid = new Grid();
            for (int i = 0; i < htmlNodeTr.Count; ++i)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(0, GridUnitType.Auto);
                grid.RowDefinitions.Add(rowDef);
            }
            for (int i = 0; i < maxCollumn; ++i)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < listTr.Count; ++i)
            {
                for (int j = 0; j < listTr[i].Count; ++j)
                {
                    listTr[i][j].SetValue(Grid.RowProperty, i);
                    listTr[i][j].SetValue(Grid.ColumnProperty, j);
                    grid.Children.Add(listTr[i][j]);
                }
            }
            resultListControl.Add(grid);
        }
        /// <summary>
        /// Method for parsing 'strong' HTML element
        /// </summary>
        /// <param name="strongNode">HTML fragment</param>
        public void ParseStrong(HtmlNode strongNode)
        {
            if (string.IsNullOrWhiteSpace(strongNode.InnerText))
                return;
            if (currentRichTextBox == null)
            {
                CreateRichTextBox();
            }
            Run run = new Run();
            run.FontSize = textFontSize;
            run.Foreground = new SolidColorBrush(textColor);
            run.FontWeight = FontWeights.Normal;
            run.Text = HtmlEntity.DeEntitize(strongNode.InnerText + " ");

            currentParagraph.Inlines.Add(run);
        }
        /// <summary>
        /// Method for parsing 'span' HTML element
        /// </summary>
        /// <param name="spanNode">HTML fragment</param>
        public void ParseSpan(HtmlNode spanNode)
        {
            ParseTags(spanNode);

            if (spanNode.Attributes.Count > 0 && spanNode.Attributes.Contains("class"))
            {
                foreach (HtmlAttribute attribute in spanNode.Attributes)
                    if (attribute.Name == "class")
                    {
                        switch (attribute.Value)
                        {
                            case "arrow":
                                string emt = string.Empty;
                                return;
                            case "number":
                                if (!CheckIsEmptyBlock())
                                {
                                    bool isFirst = false;
                                    for (int i = 0; i < currentParagraph.Inlines.Count; ++i)
                                    {
                                        Inline inline = currentParagraph.Inlines[i];
                                        if (inline is Run && !isFirst)
                                        {
                                            isFirst = true;
                                            ((Run)inline).FontSize = 50;
                                            ((Run)inline).Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)); //new SolidColorBrush(Color.FromArgb(255, 177, 0, 0));
                                            //  ((Run)inline).TextDecorations = TextDecorations.Underline;
                                        }
                                        else if (inline is LineBreak)
                                        {
                                            currentParagraph.Inlines.RemoveAt(i);
                                            --i;
                                        }
                                    }
                                    Grid grid = new Grid();
                                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(80, GridUnitType.Pixel) });
                                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(376, GridUnitType.Pixel) });

                                    if (spanNode.ParentNode.Element("div") != null && spanNode.ParentNode.Element("div").Element("p") != null)
                                    {
                                        TextBlock textBlock = new TextBlock();
                                        textBlock.TextWrapping = TextWrapping.Wrap;
                                        textBlock.FontSize = 30;
                                        textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                                        textBlock.Text = spanNode.ParentNode.Element("div").Element("p").InnerText;
                                        textBlock.VerticalAlignment = VerticalAlignment.Center;
                                        textBlock.SetValue(Grid.ColumnProperty, 1);
                                        textBlock.FontWeight = FontWeights.Thin;
                                        grid.Children.Add(textBlock);
                                    }
                                    currentRichTextBox.Blocks.Add(currentParagraph);
                                    currentRichTextBox.FontWeight = FontWeights.Thin;
                                    // grid.RenderTransform = new ScaleTransform() { ScaleX = 2.2, ScaleY = 2.2 };
                                    grid.Background = new SolidColorBrush(Colors.White); // new SolidColorBrush(Color.FromArgb(255, 210, 210, 210));

                                    currentRichTextBox.SetValue(Grid.ColumnProperty, 0);
                                    grid.Children.Add(currentRichTextBox);

                                    resultListControl.Add(grid);
                                    CreateRichTextBox();
                                }
                                break;

                        }
                    }
            }
        }
        /// <summary>
        /// Method for parsing thematic brake in HTML
        /// </summary>
        public void ParseHr()
        {
            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();
            Rectangle rect = new Rectangle();
            rect.HorizontalAlignment = HorizontalAlignment.Stretch;
            rect.Height = horizontalLineHeight;
            rect.Fill = new SolidColorBrush(horizontalLineColor);

            resultListControl.Add(rect);
        }
        /// <summary>
        /// Method for parsing images in HTML
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseImg(HtmlNode htmlNode)
        {
            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();

            Image image = new Image();
            image.Margin = imageControlMargin;

            image.HorizontalAlignment = HorizontalAlignment.Stretch;

            HtmlAttribute htmlAttribute = htmlNode.Attributes.AttributesWithName("src").First<HtmlAttribute>();
            string href = htmlAttribute.Value;
            if (!htmlAttribute.Value.Contains("http"))
            {
                href = "http://media.castorama.pl/media/" + href;
            }
            href = href.Replace("test.castorama.pl", "media.castorama.pl");
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(href, UriKind.Absolute);
            image.Source = bitmapImage;

            resultListControl.Add(image);
        }

        /// <summary>
        /// Method for parsing images in HTML
        /// </summary>
        /// <param name="href">link to image</param>
        private void ParseImg(string href)
        {
            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();

            Image image = new Image();
            image.HorizontalAlignment = HorizontalAlignment.Stretch;
            image.Margin = imageControlMargin;
            if (href.Replace("http", "").Length == href.Length)
            {
                href = "http://media.castorama.pl/media/" + href;
            }
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.CreateOptions = BitmapCreateOptions.None;
            bitmapImage.UriSource = new Uri(href, UriKind.Absolute);
            image.Source = bitmapImage;

            resultListControl.Add(image);
        }
        /// <summary>
        /// Method for parsing 'div' HTML element
        /// </summary>
        /// <param name="divNode">HTML fragment</param>
        public void ParseDiv(HtmlNode divNode)
        {

            ParseTags(divNode);

            if (!resultListControl.Contains(currentRichTextBox))
            {
                if (!CheckIsEmptyBlock())
                {
                    currentRichTextBox.Blocks.Add(currentParagraph);
                    resultListControl.Add(currentRichTextBox);
                }
                CreateRichTextBox();
            }
            else
            {
                currentParagraph.Inlines.Add(new LineBreak());
            }
        }
        /// <summary>
        /// Method for parsing italic text in HTML ('i' HTML element)
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseI(HtmlNode htmlNode)
        {
            FontStyle tempFontStyle = textFontStyle;
            textFontStyle = FontStyles.Italic;

            ParseTags(htmlNode);

            textFontStyle = tempFontStyle;

        }
        /// <summary>
        /// Method for parsing bold text in HTML ('b' HTML element)
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        public void ParseB(HtmlNode htmlNode)
        {
            FontWeight tempFontWeight = textFontWeight;
            textFontWeight = FontWeights.Normal;

            ParseTags(htmlNode);

            textFontWeight = tempFontWeight;
        }
        /// <summary>
        /// Method for parsing internal text in HTML elements
        /// </summary>
        /// <param name="text">this provided text in HTML element</param>
        private void ParseTextInternal(string text)
        {
            Run run = new Run();
            run.FontSize = textFontSize;

            bool isNumber = true;
            foreach (Char charTemp in text)
            {
                if (!char.IsNumber(charTemp) && !char.IsWhiteSpace(charTemp))
                {
                    isNumber = false;
                }
            }

            if (!isNumber)
            {
                run.Foreground = new SolidColorBrush(textColor);
            }
            else
            {
                run.Foreground = new SolidColorBrush(aHrefColor);
            }
            run.FontStyle = textFontStyle;
            run.FontWeight = textFontWeight;
            run.Text = HtmlEntity.DeEntitize(text + " ");

            currentParagraph.Inlines.Add(run);
        }
        /// <summary>
        /// Method for parsing '#text' HTML element
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseText(HtmlNode htmlNode)
        {

            if (currentRichTextBox == null)
            {
                CreateRichTextBox();
            }

            if (string.IsNullOrWhiteSpace(htmlNode.InnerText))
            {
                if (currentParagraph != null && currentParagraph.Inlines.Count > 0)
                {
                    if (currentParagraph.Inlines[currentParagraph.Inlines.Count - 1] is LineBreak)
                        return;
                }
                for (int i = 0; i < htmlNode.InnerText.Length; ++i)
                {
                    if (htmlNode.InnerText[i] == '\n')
                    {
                        currentParagraph.Inlines.Add(new LineBreak());
                        break;
                    }
                }
                return;
            }

            string text = HtmlAgilityPack.HtmlEntity.DeEntitize(htmlNode.InnerText.TrimStart(' '));

            int indexOf = -1;
            int indexEndOf = -1;
            int startIndex = 0;
            int iteration = 0;

            while ((indexOf = text.IndexOf("http://", startIndex)) != -1)
            {
                iteration++;
                indexEndOf = text.IndexOf(' ', indexOf + 7);
                if (indexEndOf > indexOf)
                {
                    startIndex = indexOf;
                }
                else
                {
                    startIndex = indexOf + 7;
                }

                if (startIndex >= text.Length)
                    break;
                if (iteration >= 10)
                    break;
            }


            if (startIndex == 0)
            {
                ParseTextInternal(text);
            }
            else
            {
                if (startIndex < text.Length)
                    ParseTextInternal(text.Substring(startIndex, text.Length - startIndex));
            }

            //currentRichTextBox.Blocks.Add(paragraph);    
        }
        /// <summary>
        /// Method for parsing paragraph HTML element
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseP(HtmlNode htmlNode)
        {

            ParseTags(htmlNode);

            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);

                resultListControl.Add(currentRichTextBox);
            }
            //if (parseRecrutationPage)
            //{
            //    textFontWeight = tempFontWeightParseRecru;
            //}
            CreateRichTextBox();
        }
        /// <summary>
        /// Method for parsing list HTML element
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseUl(HtmlNode htmlNode)
        {
            if (parseOnlyFirstListGeration && liGenerationCounter > 0)
                return;

            liGenerationCounter += 1;
            additionalSpace += liAdditionalSpaceNumber;
            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();

            //currentParagraph.Inlines.Add(new LineBreak());

            if (htmlNode.ChildNodes.Count == 0)
                ParseText(htmlNode);
            else ParseTags(htmlNode);



            additionalSpace -= liAdditionalSpaceNumber;
            liGenerationCounter -= 1;

            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();
        }
        /// <summary>
        /// Method for parsing list child HTML element
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseLi(HtmlNode htmlNode)
        {
            if (string.IsNullOrWhiteSpace(htmlNode.InnerText))
                return;
            string addSpace = "";

            //string addSpace = "";
            //for (int i = 0; i < additionalSpace; ++i)
            //    addSpace = " " + addSpace;
            if (!dontAddCharLeftPad)
                switch (liGenerationCounter)
                {
                    case 1:
                        addSpace += liCharLeftPad + " ";
                        break;
                    case 2:
                        addSpace += liCharLeftPad2 + " ";
                        break;
                    case 3:
                        addSpace += liCharLeftPad3 + " ";
                        break;
                    default:
                        addSpace += liCharLeftPad + " ";
                        break;
                }
            Run run = new Run();
            run.FontSize = textFontSize;
            run.FontWeight = textFontWeight;
            run.Foreground = new SolidColorBrush(textColor);
            run.Text = addSpace;
            run.FontStyle = textFontStyle;

            currentParagraph.Inlines.Add(run);

            ParseTags(htmlNode);
            currentParagraph.Inlines.Add(new LineBreak());

            if (addLineAfterLi)
            {
                if (!CheckIsEmptyBlock())
                {
                    currentRichTextBox.Blocks.Add(currentParagraph);
                    resultListControl.Add(currentRichTextBox);
                }

                Rectangle rect = new Rectangle();
                rect.HorizontalAlignment = HorizontalAlignment.Stretch;
                //rect.Width = controlsWidth;
                rect.Height = horizontalLineHeight;
                rect.Fill = new SolidColorBrush(horizontalLineColor);

                resultListControl.Add(rect);

                CreateRichTextBox();

                currentParagraph.Inlines.Add(new LineBreak());
            }

            //currentRichTextBox.Blocks.Add(paragraph2);  
        }
        /// <summary>
        /// Method for parsing header HTML element (biggest font)
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseH1(HtmlNode htmlNode)
        {
            if (currentParagraph.Inlines.Count > 0 && !(currentParagraph.Inlines[currentParagraph.Inlines.Count - 1] is LineBreak))
                currentParagraph.Inlines.Add(new LineBreak());

            int tempFontSize = textFontSize;
            textFontSize = h1TextFontSize;
            Color tempTextColor = textColor;

            textColor = hForegroundColor;
            currentRichTextBox.FontSize = h1TextFontSize;

            if (htmlNode.ChildNodes.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                {
                    if (currentRichTextBox == null)
                    {
                        CreateRichTextBox();
                    }

                    Run run = new Run();
                    run.FontSize = textFontSize;
                    run.Foreground = new SolidColorBrush(hForegroundColor);
                    run.FontWeight = FontWeights.Normal;
                    run.FontStyle = textFontStyle;
                    run.Text = HtmlEntity.DeEntitize(htmlNode.InnerText + " ");

                    currentParagraph.Inlines.Add(run);
                }
            }
            else ParseTags(htmlNode);

            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();

            textColor = tempTextColor;
            textFontSize = tempFontSize;
        }
        /// <summary>
        /// Method for parsing header HTML element
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseH2(HtmlNode htmlNode)
        {
            if (currentParagraph.Inlines.Count > 0 && !(currentParagraph.Inlines[currentParagraph.Inlines.Count - 1] is LineBreak))
                currentParagraph.Inlines.Add(new LineBreak());

            int tempFontSize = textFontSize;
            textFontSize = h2TextFontSize;
            currentRichTextBox.FontSize = h2TextFontSize;
            Color tempTextColor = textColor;
            textColor = hForegroundColor;
            if (currentRichTextBox != null)
            {
                currentRichTextBox.TextAlignment = TextAlignment.Left;
            }

            if (htmlNode.ChildNodes.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                {
                    if (currentRichTextBox == null)
                    {
                        CreateRichTextBox();
                    }
                    Run run = new Run();
                    run.FontSize = textFontSize;
                    run.Foreground = new SolidColorBrush(hForegroundColor);
                    run.FontWeight = FontWeights.Normal;
                    run.FontStyle = textFontStyle;
                    run.Text = HtmlEntity.DeEntitize(htmlNode.InnerText + " ");

                    currentParagraph.Inlines.Add(run);
                }
            }
            else ParseTags(htmlNode);

            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();
            textColor = tempTextColor;
            textFontSize = tempFontSize;
        }
        /// <summary>
        /// Method for parsing header HTML element
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseH3(HtmlNode htmlNode)
        {
            if (currentParagraph.Inlines.Count > 0 && !(currentParagraph.Inlines[currentParagraph.Inlines.Count - 1] is LineBreak))
                currentParagraph.Inlines.Add(new LineBreak());

            int tempFontSize = textFontSize;
            textFontSize = h3TextFontSize;
            currentRichTextBox.FontSize = h3TextFontSize;
            Color tempTextColor = textColor;
            textColor = hForegroundColor;

            if (htmlNode.ChildNodes.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                {
                    if (currentRichTextBox == null)
                    {
                        CreateRichTextBox();
                    }
                    Run run = new Run();
                    run.FontSize = textFontSize;
                    run.Foreground = new SolidColorBrush(hForegroundColor);
                    run.FontWeight = FontWeights.Normal;
                    run.FontStyle = textFontStyle;
                    run.Text = HtmlEntity.DeEntitize(htmlNode.InnerText + " ");

                    currentParagraph.Inlines.Add(run);
                }
            }
            else ParseTags(htmlNode);

            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();
            textColor = tempTextColor;
            textFontSize = tempFontSize;
        }
        /// <summary>
        /// Method for parsing header HTML element
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseH4(HtmlNode htmlNode)
        {
            if (currentParagraph.Inlines.Count > 0 && !(currentParagraph.Inlines[currentParagraph.Inlines.Count - 1] is LineBreak))
                currentParagraph.Inlines.Add(new LineBreak());

            int tempFontSize = textFontSize;
            textFontSize = h4TextFontSize;
            currentRichTextBox.FontSize = h4TextFontSize;
            Color tempTextColor = textColor;
            textColor = hForegroundColor;

            if (htmlNode.ChildNodes.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                {
                    if (currentRichTextBox == null)
                    {
                        CreateRichTextBox();
                    }
                    Run run = new Run();
                    run.FontSize = textFontSize;
                    run.Foreground = new SolidColorBrush(hForegroundColor);
                    run.FontWeight = FontWeights.Normal;
                    run.FontStyle = textFontStyle;
                    run.Text = HtmlEntity.DeEntitize(htmlNode.InnerText + " ");

                    currentParagraph.Inlines.Add(run);
                }
            }
            else ParseTags(htmlNode);

            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();
            textColor = tempTextColor;
            textFontSize = tempFontSize;
        }
        /// <summary>
        /// Method for parsing header HTML element (lowest font)
        /// </summary>
        /// <param name="htmlNode">HTML fragment</param>
        private void ParseH5(HtmlNode htmlNode)
        {
            if (currentParagraph.Inlines.Count > 0 && !(currentParagraph.Inlines[currentParagraph.Inlines.Count - 1] is LineBreak))
                currentParagraph.Inlines.Add(new LineBreak());

            int tempFontSize = textFontSize;
            textFontSize = h5TextFontSize;
            currentRichTextBox.FontSize = h5TextFontSize;
            Color tempTextColor = textColor;
            textColor = hForegroundColor;

            if (htmlNode.ChildNodes.Count == 0)
            {
                if (!string.IsNullOrWhiteSpace(htmlNode.InnerText))
                {
                    if (currentRichTextBox == null)
                    {
                        CreateRichTextBox();
                    }
                    Run run = new Run();
                    run.FontSize = textFontSize;
                    run.Foreground = new SolidColorBrush(hForegroundColor);
                    run.FontWeight = FontWeights.Normal;
                    run.FontStyle = textFontStyle;
                    run.Text = HtmlEntity.DeEntitize(htmlNode.InnerText + " ");

                    currentParagraph.Inlines.Add(run);
                }
            }
            else ParseTags(htmlNode);

            if (!CheckIsEmptyBlock())
            {
                currentRichTextBox.Blocks.Add(currentParagraph);
                resultListControl.Add(currentRichTextBox);
            }
            CreateRichTextBox();
            textColor = tempTextColor;
            textFontSize = tempFontSize;
        }
        /// <summary>
        /// Method for checking current line of Paragraph is empty
        /// </summary>
        /// <returns>True or false</returns>
        private bool CheckIsEmptyBlock()
        {
            string tempText = string.Empty;
            foreach (Inline inline in currentParagraph.Inlines)
            {
                if (inline is Run)
                {
                    tempText += ((Run)inline).Text;
                }
                else if (inline is Hyperlink)
                {
                    if (string.IsNullOrWhiteSpace(tempText))
                    {
                        return CheckIsEmptyInline(((Hyperlink)inline).Inlines);
                    }
                    else return false;
                }
            }
            return string.IsNullOrWhiteSpace(tempText);
        }
        /// <summary>
        /// Method for checking if any inline in provided collection is empty
        /// </summary>
        /// <param name="inlineCollection">Provided inline collection</param>
        /// <returns>True or false</returns>
        private bool CheckIsEmptyInline(InlineCollection inlineCollection)
        {
            string tempText = string.Empty;
            foreach (Inline inline in inlineCollection)
            {
                if (inline is Run)
                {
                    tempText += ((Run)inline).Text;
                }
                else if (inline is Hyperlink)
                {
                    if (string.IsNullOrWhiteSpace(tempText))
                    {
                        return CheckIsEmptyInline(((Hyperlink)inline).Inlines);
                    }
                    else return false;
                }
            }
            return string.IsNullOrWhiteSpace(tempText);
        }

    }
}
