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
using System.IO;

namespace Zahlensysteme
{
    public partial class MainWindow : Window
    {
    
        public void FillCombo(ComboBox a)
        {
            for (int i = 2; i < 17; i++)
            {
                if (i == 2) { a.Items.Add(i + " (Binär)"); i++; }
                else if (i == 10) { a.Items.Add(i + " (Dezimal)"); i++; }
                else if (i == 16) { a.Items.Add(i + " (Hexadezimal)"); break; }

                a.Items.Add(i);
            }
        }
      
        string ConvertDectoOtherSys(long x, int value)
        {
            return x > 0 ? x % value > 9 && x % value < value ? ConvertDectoOtherSys(x / value, value) + (char)(x % value + 55) : ConvertDectoOtherSys(x / value, value) + x % value : "";
        }
        long ConvertSysToDecimal(string x, int value, int n)
        {
            return n == x.Length ? 0 : (x[x.Length - 1 - n] > 64 && x[x.Length - 1 - n] < 91) ? ConvertSysToDecimal(x, value, n + 1) + (int)(Math.Pow(value, n)) * (int)(x[x.Length - 1 - n] - 55) :
            x[x.Length - 1 - n] > 96 && x[x.Length - 1 - n] < 123 ? ConvertSysToDecimal(x, value, n + 1) + (int)(Math.Pow(value, n)) * (int)(x[x.Length - 1 - n] - 87) : ConvertSysToDecimal(x, value, n + 1) + (int)(Math.Pow(value, n)) * (int)(x[x.Length - 1 - n] - 48);
        } 
        public bool ValidateInput(string x)
        {
            bool ch = true;
            for (int i = 0; i < x.Length; i++)
            {
                if (!((x[i] >= 48 && x[i] <= 57) || (x[i] >= 97 && x[i] <= 122) || (x[i] <= 90 && x[i] >= 65))) return false;
            }
            return true;
        }
        public string ExplanationBuilder1(long x, int sys)
        {
            string s = "", res = ""; long temp = x; s += "Weil --->\n\n";
            while (x > 0)
            {
                s += x + " / " + sys + " = " + x / sys;
                if (x % sys > 9) { s += " Rest  " + x % sys + " geschrieben als " + (char)(x % sys + 55) + "\n"; res += (char)(x % sys + 55); }
                else { s += " Rest - " + x % sys + "\n"; res += x % sys; }
                x /= sys;
            }
            s += "\n";
            return s + "deshalb " + temp + " gleich " + new string(res.Reverse().ToArray()) + "\n-----------------------------------------------------------\n";
        }
        public string ExplanationBuilder2(string x, int sys)
        {
            string s = "", res = ""; var arr = new int[x.Length]; int erg = 0;

            s += "A/a=10 B/b=11 C/c=12 D/d=13 E/e=14 F/f=15\n\n\ngestartet wird immer von hinten\nbei " + x + " starten wir also mit " + x[x.Length - 1] + "\n\n";

            for (int i = 0; i < x.Length; i++)
            {
                if (x[x.Length - 1 - i] > 64 && x[x.Length - 1 - i] < 91)
                {

                    s += x[x.Length - 1 - i] + " - (" + sys + " hoch " + i + ") * " + (int)(x[x.Length - 1 - i] - 55) + " = " + ((int)(Math.Pow(sys, i)) * (int)(x[x.Length - 1 - i] - 55)) + "\n"; arr[i] = ((int)(Math.Pow(sys, i)) * (int)(x[x.Length - 1 - i] - 55));
                }
                else if (x[x.Length - 1 - i] > 96 && x[x.Length - 1 - i] < 123)
                {

                    s += x[x.Length - 1 - i] + " - (" + sys + " hoch " + i + ") * " + (int)(x[x.Length - 1 - i] - 87) + " = " + ((int)(Math.Pow(sys, i)) * (int)(x[x.Length - 1 - i] - 87)) + "\n"; arr[i] = ((int)(Math.Pow(sys, i)) * (int)(x[x.Length - 1 - i] - 87));
                }
                else { s += x[x.Length - 1 - i] + " - (" + sys + " hoch " + i + ") * " + (int)(x[x.Length - 1 - i] - 48) + " = " + ((int)(Math.Pow(sys, i)) * (int)(x[x.Length - 1 - i] - 48)) + "\n"; arr[i] = ((int)(Math.Pow(sys, i)) * (int)(x[x.Length - 1 - i] - 48)); }
            }
            s += "\n\n";
            for (int i = 0; i < arr.Length - 1; i++)
            {
                res += arr[i] + " + "; erg += arr[i];
            }
            res += arr[arr.Length - 1] + " = " + (erg + arr[arr.Length - 1]);
            return s + res + "\n-----------------------------------------------------------\n";
        }

        public MainWindow()
        {
            InitializeComponent();
            FillCombo(combo1); FillCombo(combo2);
        }

        private void combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (text2.Text != "") text1.Text = text1.Text = ConvertDectoOtherSys(ConvertSysToDecimal(text2.Text, (int)(combo2.SelectedIndex + 2), 0), (int)(combo1.SelectedIndex + 2));
        }

        private void combo2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (text1.Text != "") text2.Text = ConvertDectoOtherSys(ConvertSysToDecimal(text1.Text, (int)(combo1.SelectedIndex + 2), 0), (int)(combo2.SelectedIndex + 2));
        }
        public bool textfocus;
        private void text1_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (text1.Text != "" && !text2.IsFocused)
            {
                text2.Text = ConvertDectoOtherSys(ConvertSysToDecimal(text1.Text, combo1.SelectedIndex + 2, 0), combo2.SelectedIndex + 2);
                foreach (var item in text1.Text)
                {
                    if (((int)(item - 48) > combo1.SelectedIndex + 1 && combo1.SelectedIndex < 9) ||
                           (((item > 96 && item < 123) || (item > 64 && item < 91)) && combo1.SelectedIndex < 9)
                    || (combo1.SelectedIndex > 8 && item > combo1.SelectedIndex + 88 && item < combo1.SelectedIndex + 113)
                    || (combo1.SelectedIndex > 8 && item > combo1.SelectedIndex + 56 && item < combo1.SelectedIndex + 79))
                        text2.Text = "NaN";
                }
            }
            if (text1.Text != "" && (text2.Text == "" || !ValidateInput(text1.Text))) text2.Text = "error";
            textfocus = true;
            if (check.IsChecked == true) Button_Click(sender, e);
        }

        private void text2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (text2.Text != "" && !text1.IsFocused)
            {
                text1.Text = ConvertDectoOtherSys(ConvertSysToDecimal(text2.Text, combo2.SelectedIndex + 2, 0), combo1.SelectedIndex + 2);
                foreach (var item in text2.Text)
                {
                    if (((int)(item - 48) > combo2.SelectedIndex + 1 && combo2.SelectedIndex < 9) ||
                           (((item > 96 && item < 123) || (item > 64 && item < 91)) && combo2.SelectedIndex < 9)
                           || (combo2.SelectedIndex > 8 && item > combo2.SelectedIndex + 88 && item < combo2.SelectedIndex + 113)
                    || (combo2.SelectedIndex > 8 && item > combo2.SelectedIndex + 56 && item < combo2.SelectedIndex + 79))
                        text1.Text = "NaN";
                }
            }
            if (text2.Text != "" && (text1.Text == "" || !ValidateInput(text2.Text))) text1.Text = "error";
            textfocus = false;
            if (check.IsChecked == true) Button_Click(sender, e);
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(text1.Text) && !String.IsNullOrEmpty(text2.Text))
            {
                if (!textfocus)
                {
                    if (combo2.SelectedIndex == 8) descr.Text = ExplanationBuilder1(ConvertSysToDecimal(text2.Text, combo2.SelectedIndex + 2, 0), combo1.SelectedIndex + 2);
                    else if (combo2.SelectedIndex != 8 && combo1.SelectedIndex != 8)
                    {
                        descr.Text = "Erst rechnen wir ins Dezimalsystem um\n---------------------------------------------------------- -\n" + ExplanationBuilder2(text2.Text, combo2.SelectedIndex + 2);
                        descr.Text += "\n\n" + ExplanationBuilder1(ConvertSysToDecimal(text2.Text, combo2.SelectedIndex + 2, 0), combo1.SelectedIndex + 2);
                    }
                    else descr.Text = ExplanationBuilder2(text2.Text, combo2.SelectedIndex + 2);
                }
                else
                {
                    if (combo1.SelectedIndex == 8) descr.Text = ExplanationBuilder1(ConvertSysToDecimal(text1.Text, combo1.SelectedIndex + 2, 0), combo2.SelectedIndex + 2);
                    else if (combo2.SelectedIndex != 8 && combo1.SelectedIndex != 8)
                    {
                        descr.Text = "Erst rechnen wir ins Dezimalsystem um \n---------------------------------------------------------- -\n" + ExplanationBuilder2(text1.Text, combo1.SelectedIndex + 2);
                        descr.Text += "\n\n" + ExplanationBuilder1(ConvertSysToDecimal(text1.Text, combo1.SelectedIndex + 2, 0), combo2.SelectedIndex + 2);
                    }
                    else descr.Text = ExplanationBuilder2(text1.Text, combo1.SelectedIndex + 2);

                }
                if (text1.Text == "NaN" || text2.Text == "NaN" || text1.Text == "error" || text2.Text == "error") descr.Text = "";
            }
        }
    }
}
