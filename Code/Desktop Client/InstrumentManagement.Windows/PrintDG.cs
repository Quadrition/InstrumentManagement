namespace InstrumentManagement.Windows
{
    using InstrumentManagement.Windows.Converters;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Media;

    public static class PrintDG
    {
        public static void Print<T>(DataGrid dataGrid, string title, params string[] subTitles)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument fd = new FlowDocument();

                Paragraph p = new Paragraph(new Run(title))
                {
                    FontStyle = dataGrid.FontStyle,
                    FontWeight = FontWeights.Bold,
                    FontFamily = dataGrid.FontFamily,
                    FontSize = 24
                };
                fd.Blocks.Add(p);

                for (int i = 0; i < subTitles.Length; i++)
                {
                    Paragraph sub = new Paragraph(new Run(subTitles[i]))
                    {
                        FontStyle = dataGrid.FontStyle,
                        FontFamily = dataGrid.FontFamily,
                        FontSize = 14
                    };
                    fd.Blocks.Add(sub);
                }

                Table table = new Table();
                TableRowGroup tableRowGroup = new TableRowGroup();
                TableRow r = new TableRow();
                fd.PageWidth = printDialog.PrintableAreaWidth - 15;
                fd.PageHeight = printDialog.PrintableAreaHeight - 15;
                fd.BringIntoView();

                fd.TextAlignment = TextAlignment.Center;
                fd.ColumnWidth = 500;
                table.CellSpacing = 0;

                var headerList = dataGrid.Columns.Select(e => e.Header.ToString()).ToList();
                List<string> bindList = new List<string>();


                for (int j = 0; j < headerList.Count; j++)
                {

                    r.Cells.Add(new TableCell(new Paragraph(new Run(headerList[j]))));
                    r.Cells[j].ColumnSpan = 4;
                    r.Cells[j].Padding = new Thickness(4);
                    r.Cells[j].TextAlignment = TextAlignment.Center;

                    r.Cells[j].BorderBrush = Brushes.Black;
                    r.Cells[j].Background = Brushes.DarkGray;
                    r.Cells[j].Foreground = Brushes.White;
                    r.Cells[j].BorderThickness = new Thickness(1, 1, 1, 1);
                    var binding = (dataGrid.Columns[j] as DataGridBoundColumn).Binding as Binding;

                    bindList.Add(binding.Path.Path);
                }
                tableRowGroup.Rows.Add(r);
                table.RowGroups.Add(tableRowGroup);
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {

                    dynamic row;
                    if (dataGrid.ItemsSource.ToString().ToLower() == "system.data.linqdataview")
                    { row = (DataRowView)dataGrid.Items.GetItemAt(i); }
                    else
                    {
                        row = (T)dataGrid.Items.GetItemAt(i);

                    }

                    table.BorderBrush = Brushes.Black;
                    table.BorderThickness = new Thickness(1, 1, 0, 0);
                    table.FontStyle = dataGrid.FontStyle;
                    table.FontFamily = dataGrid.FontFamily;
                    table.FontSize = 13;
                    tableRowGroup = new TableRowGroup();
                    r = new TableRow();

                    for (int j = 0; j < bindList.Count; j++)
                    {

                        if (dataGrid.ItemsSource.ToString().ToLower() == "system.data.linqdataview")
                        {
                            r.Cells.Add(new TableCell(new Paragraph(new Run(row.Row.ItemArray[j].ToString()))));

                        }
                        else
                        {
                            dynamic value = null;

                            if (bindList[j].Split('.').Length != 1)
                            {
                                string[] paths = bindList[j].Split('.');
                                value = row.GetType().GetProperty(paths[0]).GetValue(row, null);
                                for (int k = 1; k < paths.Length; k++)
                                {
                                    value = value.GetType().GetProperty(paths[k]).GetValue(value, null);
                                }
                            }
                            else
                            {
                                value = row.GetType().GetProperty(bindList[j]).GetValue(row, null);
                            }

                            if (value is DateTime)
                            {
                                value = value.ToShortDateString();
                            }
                            else if (value is bool)
                            {
                                BoolTranslationConverter converter = new BoolTranslationConverter();
                                value = converter.Convert(value, null, null, null);
                            }
                            else
                            {
                                value = Convert.ToString(value);
                            }

                            r.Cells.Add(new TableCell(new Paragraph(new Run(value))));
                        }

                        r.Cells[j].ColumnSpan = 4;
                        r.Cells[j].Padding = new Thickness(2);

                        r.Cells[j].BorderBrush = Brushes.DarkGray;
                        r.Cells[j].BorderThickness = new Thickness(0, 0, 1, 1);
                    }

                    tableRowGroup.Rows.Add(r);
                    table.RowGroups.Add(tableRowGroup);

                }
                fd.Blocks.Add(table);

                printDialog.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "");

            }
        }
    }
}
