﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Data;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace AuroraVisionLauncher.Converters;
public class CommandlineToFormattedTextConverter : IValueConverter
{
    private static readonly Regex Tokenizer = new("(\"[^\"]+\"|\\S+)");
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string input)
        {
            return DependencyProperty.UnsetValue;
        }

        var para = new Paragraph();
        para.TextAlignment = TextAlignment.Left;

        // Example logic to split and format parts of the text
        var parts = Tokenizer.Matches(input).Select(x => x.Value).ToList();

        if (parts.Count == 0)
        {
            return new FlowDocument(para);
        }

        var exeRun = new Run(parts[0]);
        exeRun.Foreground = Brushes.PaleGreen;

        para.Inlines.Add(exeRun);

        foreach (var part in parts.Skip(1))
        {
            para.Inlines.Add(new Run(" "));
            var run = new Run(part);
            if (part.StartsWith('-'))
            {
                run.FontStyle = FontStyles.Italic;
                run.Foreground = Brushes.Cyan;
            }
            else
            {
                run.FontWeight = FontWeights.Bold;
                run.Foreground = Brushes.MediumPurple;
            }
            para.Inlines.Add(run);
        }
        var doc = new FlowDocument(para)
        {
            PagePadding = new Thickness(0),
            TextAlignment = TextAlignment.Left,
            LineHeight = double.NaN,
            IsOptimalParagraphEnabled = false,
            IsColumnWidthFlexible = false,
            IsHyphenationEnabled = false,
            
        };
        return doc;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}