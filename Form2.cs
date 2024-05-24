using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        // Method to add a new line in the panel
        void NewLine(FlowLayoutPanel panel)
        {
            Panel NewLine = new Panel
            {
                Height = 0,
                Width = 0,
                Dock = DockStyle.Bottom,
                Margin = new Padding(0),
            };
            panel.SetFlowBreak(NewLine, true);
            panel.Controls.Add(NewLine);
        }

        // Method to add text to the panel
        void AddText(FlowLayoutPanel panel, string text, Color color)
        {
            panel.Controls.Add(new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Width = MainPanel.Width - 5,
                ForeColor = color,
            });
            NewLine(MainPanel);
        }

        // Method to display the linear programming problem
        void ShowTheProblem(FlowLayoutPanel panel)
        {
            string line = "";
            if (Form1.isMax) line += "Maximize Z(X) =  ";
            else line += "Minimize Z(X) =  ";
            line += ReturnEquation(Form1.ProblemVar);
            AddText(panel, line, Color.Black);
        }

        // Method to display the problem constraints
        void ShowConstraints(FlowLayoutPanel panel)
        {
            string line = "";
            for (int i = 0; i < Form1.ProblemCons.Count; i++)
            {
                line += ReturnEquation(Form1.ProblemCons[i]);
                line += "   " + Form1.ProblemDirection[i] + "   " + Form1.RightVal[i];
                AddText(panel, line, Color.Black);
                line = "";
            }
            line = "";
            for (int i = 0; i < Form1.ProblemVar.Count; i++)
            {
                line += " X" + (i + 1) + ",";
            }
            char[] charsToTrim = { ' ', ',' };
            line = line.TrimEnd(charsToTrim);
            AddText(panel, "While " + line + " > 0", Color.Black);
        }


        // Method to return the new simplex table
        List<List<double>> ReturnTheNewTable(List<List<double>> C, List<string> D, List<double> R)
        {
            List<List<double>> newTab = new List<List<double>>();
            for (int i = 0; i < C.Count; i++)
            {
                newTab.Add(new List<double>());
                for (int j = 0; j < C[i].Count; j++)
                {
                    newTab[i].Add(C[i][j]);
                }
            }
            // Add slack and artificial variables
            for (int i = 0; i < D.Count; i++)
            {
                if (D[i] == "≤" || D[i] == "≥")
                {
                    for (int j = 0; j < D.Count; j++)
                    {
                        if (j == i)
                        {
                            if (D[i] == "≤")
                                newTab[j].Add(1);
                            else newTab[j].Add(-1);
                        }
                        else newTab[j].Add(0);
                    }
                }
            }

            for (int i = 0; i < D.Count; i++)
            {
                if (D[i] == "=" || D[i] == "≥")
                {
                    for (int j = 0; j < D.Count; j++)
                    {
                        if (j == i)
                            newTab[j].Add(1);
                        else newTab[j].Add(0);
                    }
                }
            }

            for (int i = 0; i < D.Count; i++)
            {
                newTab[i].Add(R[i]);
            }
            newTab.Add(new List<double>());
            for (int i = 0; i < newTab[0].Count; i++)
            {
                if (i < Form1.ProblemVar.Count)
                {
                    newTab[C.Count].Add(Form1.ProblemVar[i]);
                }
                else newTab.Last().Add(0);
            }
            if (NeedAV())
            {
                newTab.Add(new List<double>());
                for (int i = 0; i < newTab[0].Count; i++)
                {
                    double M = 0;
                    for (int j = 0; j < D.Count; j++)
                    {
                        if (i < newTab[0].Count - AVcount() - 1 || i == newTab[0].Count - 1)
                        {
                            if (D[j] != "≤")
                            {
                                M += newTab[j][i];
                            }
                        }
                    }
                    if (Form1.isMax)
                        newTab.Last().Add(M);
                    else
                        newTab.Last().Add(M * -1);
                }
            }
            return newTab;
        }

        // Method to return all variables in the problem
        List<string> ReturnAllVariables(List<List<double>> C, List<string> D)
        {
            List<string> Vars = new List<string>();

            // Add original variables
            // Add slack variables
            // Add artificial variables
            for (int i = 0; i < C[0].Count; i++)
            {
                Vars.Add("X" + (i + 1));
            }

            int k = 0;
            for (int i = 0; i < D.Count; i++)
            {
                if (D[i] == "≤" || D[i] == "≥")
                {
                    k++;
                    Vars.Add("S" + k);
                }
            }

            k = 0;
            for (int i = 0; i < D.Count; i++)
            {
                if (D[i] == "=" || D[i] == "≥")
                {
                    k++;
                    Vars.Add("A" + k);
                }
            }
            Vars.Add("RHS");
            return Vars;
        }

        // Method to display the standard form of the problem
        void ShowStandardForm(FlowLayoutPanel panel)
        {
            // Get the new simplex table and all variables
            List<List<double>> newTab = ReturnTheNewTable(Form1.ProblemCons, Form1.ProblemDirection, Form1.RightVal);
            List<string> AllVars = ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection);
            string line = "";

            // Loop through the table to display the standard equations
            if (!NeedAV())
            {
                for (int i = 0; i < newTab.Count - 1; i++)
                {
                    line += ReturnStandardEquation(newTab[i], AllVars);
                    AddText(panel, line, Color.Black);
                    line = "";
                }
            }
            else
            {
                for (int i = 0; i < newTab.Count - 2; i++)
                {
                    line += ReturnStandardEquation(newTab[i], AllVars);
                    AddText(panel, line, Color.Black);
                    line = "";
                }
            }

            // Display the constraint for non-negative variables
            line = "While";
            for (int i = 0; i < AllVars.Count - 1; i++)
            {
                line += "  " + AllVars[i] + ",";
            }
            char[] charsToTrim = { ' ', ',' };
            line = line.TrimEnd(charsToTrim);
            line += " ≥ 0";
            AddText(panel, line, Color.Black);
        }

        // Method to return the equation represented by a list of coefficients
        string ReturnEquation(List<double> l)
        {
            string Eq = "";
            for (int i = 0; i < l.Count; i++)
            {
                if (i == l.Count - 1)
                {
                    if (l[i] != 0)
                    {
                        if (Eq != "")
                            Eq += Math.Abs(l[i]) + " X" + (i + 1);
                        else Eq += l[i] + " X" + (i + 1);
                    }
                }
                else
                {
                    if (l[i] != 0)
                    {
                        if (l[i + 1] > 0)
                        {
                            if (Eq == "")
                                Eq += l[i] + " X" + (i + 1) + "   +   ";
                            else
                                Eq += Math.Abs(l[i]) + " X" + (i + 1) + "   +   ";
                        }
                        else if (l[i + 1] < 0)
                        {
                            if (Eq == "")
                                Eq += l[i] + " X" + (i + 1) + "   -   ";
                            else
                                Eq += Math.Abs(l[i]) + " X" + (i + 1) + "   -   ";
                        }
                        else if (l[i + 1] == 0)
                        {
                            for (int j = i + 1; j < l.Count; j++)
                            {
                                if (l[j] > 0)
                                {
                                    if (Eq == "")
                                    {
                                        Eq += l[i] + " X" + (i + 1) + "   +   ";
                                        break;
                                    }
                                    else
                                    {
                                        Eq += Math.Abs(l[i]) + " X" + (i + 1) + "   +   ";
                                        break;
                                    }
                                }
                                else if (l[j] < 0)
                                {
                                    if (Eq == "")
                                    {
                                        Eq += l[i] + " X" + (i + 1) + "   -   ";
                                        break;
                                    }
                                    else
                                    {
                                        Eq += Math.Abs(l[i]) + " X" + (i + 1) + "   -   ";
                                        break;
                                    }
                                }
                                else if (i == l.Count - 2)
                                {
                                    if (Eq == "")
                                    {
                                        Eq += l[i] + " X" + (i + 1);
                                        break;
                                    }
                                    else
                                    {
                                        Eq += Math.Abs(l[i]) + " X" + (i + 1);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            char[] charsToTrim = { ' ', '+', '-' };
            Eq = Eq.TrimEnd(charsToTrim);

            return Eq;
        }

        // Method to return the standard equation represented by a list of coefficients and variables
        string ReturnStandardEquation(List<double> l, List<string> v)
        {
            string Eq = "";
            for (int i = 0; i < l.Count - 1; i++)
            {
                if (i == l.Count - 1)
                {
                    if (l[i] != 0)
                    {
                        if (Eq != "")
                            Eq += Math.Abs(l[i]) + " " + v[i];
                        else Eq += l[i] + " " + v[i];
                    }
                }
                else
                {
                    if (l[i] != 0)
                    {
                        if (l[i + 1] > 0)
                        {
                            if (Eq == "")
                                Eq += l[i] + " " + v[i] + "   +   ";
                            else
                                Eq += Math.Abs(l[i]) + " " + v[i] + "   +   ";
                        }
                        else if (l[i + 1] < 0)
                        {
                            if (Eq == "")
                                Eq += l[i] + " " + v[i] + "   -   ";
                            else
                                Eq += Math.Abs(l[i]) + " " + v[i] + "   -   ";
                        }
                        else if (l[i + 1] == 0)
                        {
                            for (int j = i + 1; j < l.Count; j++)
                            {
                                if (l[j] > 0)
                                {
                                    if (Eq == "")
                                    {
                                        Eq += l[i] + " " + v[i] + "   +   ";
                                        break;
                                    }
                                    else
                                    {
                                        Eq += Math.Abs(l[i]) + " " + v[i] + "   +   ";
                                        break;
                                    }
                                }
                                else if (l[j] < 0)
                                {
                                    if (Eq == "")
                                    {
                                        Eq += l[i] + " " + v[i] + "   -   ";
                                        break;
                                    }
                                    else
                                    {
                                        Eq += Math.Abs(l[i]) + " " + v[i] + "   -   ";
                                        break;
                                    }
                                }
                                else if (i == l.Count - 2)
                                {
                                    if (Eq == "")
                                    {
                                        Eq += l[i] + " " + v[i];
                                        break;
                                    }
                                    else
                                    {
                                        Eq += Math.Abs(l[i]) + " " + v[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            char[] charsToTrim = { ' ', '+', '-' };
            Eq = Eq.TrimEnd(charsToTrim);
            if (l.Any())
                Eq += "   =   " + l.Last();

            return Eq;
        }


        // Method to return the variables corresponding to the left-hand side of constraints
        List<string> ReturnLeftVars(List<string> D)
        {
            List<string> vars = new List<string>();
            int k = 0,
                l = 0;
            for (int i = 0; i < D.Count; i++)
            {
                if (D[i] == "≥")
                {
                    vars.Add("A" + (k + 1));
                    k++;
                    l++;
                }
                else if (D[i] == "=")
                {
                    vars.Add("A" + (k + 1));
                    k++;
                }
                else
                {
                    if (k != 0)
                    {
                        vars.Add("S" + (l + 1));
                        l++;
                    }
                    else
                    {
                        vars.Add("S" + (i + 1));
                        l++;
                    }
                }
            }
            return vars;
        }

        // Method to display the first table of the simplex method
        void ShowFirstTab(FlowLayoutPanel panel, List<List<double>> table, List<string> allVars, List<string> leftVars)
        {

            // Add labels for variables and Z column
            panel.Controls.Add(new TextBox
            {
                Font = new Font("Segoe UI", 12),
                TextAlign = HorizontalAlignment.Center,
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = Color.Snow,
                Width = 90,
                ReadOnly = true,
                Enabled = false,
            });
            for (int i = 0; i < allVars.Count; i++)
            {
                panel.Controls.Add(new TextBox
                {
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    TextAlign = HorizontalAlignment.Center,
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.LightSteelBlue,
                    Width = 90,
                    ReadOnly = true,
                    Text = allVars[i],
                });
            }
            NewLine(panel);

            // Add variables and coefficients
            if (!NeedAV())
            {
                for (int i = 0; i < table.Count; i++)
                {
                    if (i < leftVars.Count)
                        panel.Controls.Add(new TextBox
                        {
                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                            TextAlign = HorizontalAlignment.Center,
                            BorderStyle = BorderStyle.Fixed3D,
                            BackColor = Color.LightSteelBlue,
                            Width = 90,
                            ReadOnly = true,
                            Text = leftVars[i],
                        });
                    else
                    {

                        // Add Max Z or Min Z label
                        if (Form1.isMax)
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.LightSteelBlue,
                                Width = 90,
                                ReadOnly = true,
                                Text = "Max Z"
                            });
                        else
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.LightSteelBlue,
                                Width = 90,
                                ReadOnly = true,
                                Text = "Min Z"
                            });
                    }

                    // Add coefficients
                    for (int j = 0; j < table[i].Count; j++)
                    {
                        panel.Controls.Add(new TextBox
                        {
                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                            TextAlign = HorizontalAlignment.Center,
                            BorderStyle = BorderStyle.Fixed3D,
                            BackColor = Color.WhiteSmoke,
                            Width = 90,
                            ReadOnly = true,
                            Text = table[i][j].ToString(),
                        });
                    }
                    NewLine(panel);
                }
            }
            else
            {

                // Add variables and coefficients, including artificial variable (M)
                for (int i = 0; i < table.Count - 1; i++)
                {
                    if (i < leftVars.Count)
                        panel.Controls.Add(new TextBox
                        {
                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                            TextAlign = HorizontalAlignment.Center,
                            BorderStyle = BorderStyle.Fixed3D,
                            BackColor = Color.LightSteelBlue,
                            Width = 90,
                            ReadOnly = true,
                            Text = leftVars[i],
                        });
                    else
                    {
                        // Add Max Z or Min Z label
                        if (Form1.isMax)
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.LightSteelBlue,
                                Width = 90,
                                ReadOnly = true,
                                Text = "Max Z"
                            });
                        else
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.LightSteelBlue,
                                Width = 90,
                                ReadOnly = true,
                                Text = "Min Z"
                            });
                    }

                    // Add coefficients with artificial variables and 'M'
                    for (int j = 0; j < table[i].Count; j++)
                    {
                        if (i != table.Count - 2)
                        {
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.WhiteSmoke,
                                Width = 90,
                                ReadOnly = true,
                                Text = table[i][j].ToString(),
                            });
                        }
                        else
                        {
                            // Handle artificial variable with 'M'
                            if (table[i][j] != 0)
                            {
                                if (table[i + 1][j] != 0)
                                {
                                    panel.Controls.Add(new TextBox
                                    {
                                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                        TextAlign = HorizontalAlignment.Center,
                                        BorderStyle = BorderStyle.Fixed3D,
                                        BackColor = Color.WhiteSmoke,
                                        Width = 90,
                                        ReadOnly = true,
                                        Text = table[i][j].ToString() + "+(" + table[i + 1][j].ToString() + "M)",
                                    });
                                }
                                else
                                {
                                    panel.Controls.Add(new TextBox
                                    {
                                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                        TextAlign = HorizontalAlignment.Center,
                                        BorderStyle = BorderStyle.Fixed3D,
                                        BackColor = Color.WhiteSmoke,
                                        Width = 90,
                                        ReadOnly = true,
                                        Text = table[i][j].ToString(),
                                    });
                                }
                            }
                            else
                            {

                                // Handle only 'M'
                                if (table[i + 1][j] != 0)
                                {
                                    panel.Controls.Add(new TextBox
                                    {
                                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                        TextAlign = HorizontalAlignment.Center,
                                        BorderStyle = BorderStyle.Fixed3D,
                                        BackColor = Color.WhiteSmoke,
                                        Width = 90,
                                        ReadOnly = true,
                                        Text = "(" + table[i + 1][j].ToString() + "M)",
                                    });
                                }
                                else
                                {
                                    // If both are zero, just add the coefficient
                                    panel.Controls.Add(new TextBox
                                    {
                                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                        TextAlign = HorizontalAlignment.Center,
                                        BorderStyle = BorderStyle.Fixed3D,
                                        BackColor = Color.WhiteSmoke,
                                        Width = 90,
                                        ReadOnly = true,
                                        Text = table[i][j].ToString(),
                                    });
                                }
                            }
                        }
                    }
                    NewLine(panel);
                }
            }
        }

        //next
        bool NeedAV()
        {
            // This method checks if artificial variables are needed in the problem formulation.
            // It iterates through the list of constraint directions.
            for (int i = 0; i < Form1.ProblemDirection.Count; i++)
            {
                // If any constraint direction is not "≤" (less than or equal), return true,
                // indicating the need for artificial variables.
                if (Form1.ProblemDirection[i] != "≤")
                    return true;
            }
            // If all constraints are of type "≤", return false, indicating no need for artificial variables.
            return false;
        }
        int AVcount()
        {
            int count = 0;
            // This method counts the number of constraints that require artificial variables.
            // It iterates through the list of constraint directions.
            for (int i = 0; i < Form1.ProblemDirection.Count; i++)
            {
                // If a constraint direction is not "≤" (less than or equal), increment the count.
                if (Form1.ProblemDirection[i] != "≤")
                    count++;
            }
            // Return the total count of constraints that require artificial variables.
            return count;
        }


        // Method to perform simplex iterations
        void SimplexIterations(FlowLayoutPanel panel, List<List<double>> table, List<string> allVars, List<string> leftVars)
        {
            int iteration;// Counter for the iteration number
            double pivot = 0; // Placeholder for the pivot element
            List<List<List<double>>> history = new List<List<List<double>>>();// List to store the history of tables
            bool Sycle = false; // Flag to indicate if a cycle is detected

            // Loop until the stopping condition is met
            for (iteration = 0; !Stop(table); iteration++)
            {
                // Check for cycle detection
                Sycle = ExistsBefore(history, table);

                if (Sycle) break;

                // Display iteration header
                AddText(MainPanel, "- Iteration " + (iteration + 1) + ":", Color.Green);

                int BaseVariableindex;

                // Determine the index of the entering basic variable (pivot column)
                if (!NeedAV())
                {
                    BaseVariableindex = maxIndex(table.Last());
                }
                else
                {
                    // Determine the entering variable based on the objective function type and table values
                    if (Form1.isMax)
                    {
                        if (!AllValuesEqual(table.Last()) && HasNonNegative(table.Last()))
                            BaseVariableindex = maxIndex(table.Last());
                        else
                            BaseVariableindex = maxIndex(table[table.Count - 2]);
                    }
                    else
                    {
                        if (!AllValuesEqual(table.Last()) && HasNonPsitive(table.Last()))
                            BaseVariableindex = maxIndex(table.Last());
                        else
                            BaseVariableindex = maxIndex(table[table.Count - 2]);
                    }
                }

                // Determine the index of the leaving basic variable (pivot row)
                int UpToBaseVariableindex = minLeftValuesIndex(table, BaseVariableindex);

                // Display table header
                // The header includes all variable names

                panel.Controls.Add(new TextBox
                {
                    Font = new Font("Segoe UI", 12),
                    TextAlign = HorizontalAlignment.Center,
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.Snow,
                    Width = 90,
                    ReadOnly = true,
                    Enabled = false,
                });
                for (int i = 0; i < allVars.Count; i++)
                {
                    panel.Controls.Add(new TextBox
                    {
                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                        TextAlign = HorizontalAlignment.Center,
                        BorderStyle = BorderStyle.Fixed3D,
                        BackColor = Color.LightSteelBlue,
                        Width = 90,
                        ReadOnly = true,
                        Text = allVars[i],
                    });
                }
                NewLine(panel);
                if (!NeedAV())
                {
                    // Display simplex table rows
                    for (int i = 0; i < table.Count; i++)
                    {
                        // Display left-hand side variables or objective function label
                        if (i < leftVars.Count)
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.LightSteelBlue,
                                Width = 90,
                                ReadOnly = true,
                                Text = leftVars[i],
                            });
                        else
                        {
                            // Display the label for the objective function (Max Z or Min Z)
                            if (Form1.isMax)
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightSteelBlue,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = "Max Z"
                                });
                            else
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightSteelBlue,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = "Min Z"
                                });
                        }
                        // Display simplex table elements
                        for (int j = 0; j < table[i].Count; j++)
                        {
                            // Highlight the pivot element
                            if (j == BaseVariableindex && i == UpToBaseVariableindex)
                            {
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightCoral,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = table[i][j].ToString("0.##"),
                                });
                            }
                            // Highlight elements involved in pivot operations
                            else if (j == BaseVariableindex || i == UpToBaseVariableindex)
                            {
                                if ((j == BaseVariableindex && i == table.Count - 1)
                                    || (j == table[i].Count - 1 && i == UpToBaseVariableindex))
                                {
                                    panel.Controls.Add(new TextBox
                                    {
                                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                        TextAlign = HorizontalAlignment.Center,
                                        BorderStyle = BorderStyle.Fixed3D,
                                        BackColor = Color.LightGreen,
                                        Width = 90,
                                        ReadOnly = true,
                                        Text = table[i][j].ToString("0.##"),
                                    });
                                }
                                else
                                {
                                    panel.Controls.Add(new TextBox
                                    {
                                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                        TextAlign = HorizontalAlignment.Center,
                                        BorderStyle = BorderStyle.Fixed3D,
                                        BackColor = Color.LightPink,
                                        Width = 90,
                                        ReadOnly = true,
                                        Text = table[i][j].ToString("0.##"),
                                    });
                                }
                            }
                            // Display regular table elements
                            else
                            {
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.WhiteSmoke,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = table[i][j].ToString("0.##"),
                                });
                            }
                        }
                        NewLine(panel);// Move to the next line after displaying a row
                    }

                }


                else
                {
                    // Loop through the table rows except for the last row
                    for (int i = 0; i < table.Count - 1; i++)
                    {
                        // Display left-hand side variables or objective function label
                        if (i < leftVars.Count)
                            // Add left-hand side variables
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.LightSteelBlue,
                                Width = 90,
                                ReadOnly = true,
                                Text = leftVars[i],
                            });
                        else
                        {
                            // Add label for the objective function (Max Z or Min Z)
                            if (Form1.isMax)
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightSteelBlue,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = "Max Z"
                                });
                            else
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightSteelBlue,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = "Min Z"
                                });
                        }
                        // Display simplex table elements
                        for (int j = 0; j < table[i].Count; j++)
                        {



                            if (i != table.Count - 2)
                            {
                                // Display regular table elements
                                if (j == BaseVariableindex && i == UpToBaseVariableindex)
                                {
                                    // Highlight the pivot element in the current iteration
                                    panel.Controls.Add(new TextBox
                                    {
                                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                        TextAlign = HorizontalAlignment.Center,
                                        BorderStyle = BorderStyle.Fixed3D,
                                        BackColor = Color.LightCoral,
                                        Width = 90,
                                        ReadOnly = true,
                                        Text = table[i][j].ToString("0.##"),
                                    });
                                }
                                else if (j == BaseVariableindex || i == UpToBaseVariableindex)
                                {
                                    // Highlight elements involved in pivot operations with different colors
                                    if ((j == BaseVariableindex && i == table.Count - 1)
                                        || (j == table[i].Count - 1 && i == UpToBaseVariableindex))
                                    {
                                        // Highlight pivot column and pivot row elements with light green
                                        panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.LightGreen,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = table[i][j].ToString("0.##"),
                                        });
                                    }
                                    else
                                    {
                                        // Highlight other elements involved in pivot operations with light pink
                                        panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.LightPink,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = table[i][j].ToString("0.##"),
                                        });
                                    }
                                }
                                else
                                {
                                    // Display regular table elements
                                    panel.Controls.Add(new TextBox
                                    {
                                        Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                        TextAlign = HorizontalAlignment.Center,
                                        BorderStyle = BorderStyle.Fixed3D,
                                        BackColor = Color.WhiteSmoke,
                                        Width = 90,
                                        ReadOnly = true,
                                        Text = table[i][j].ToString("0.##"),
                                    });
                                }
                            }
                            else
                            {
                                // Display elements for the last two rows (last iteration)
                                if (table[i][j] != 0)
                                {
                                    if (table[i + 1][j] != 0)
                                    {
                                        // Display non-zero elements with 'M' for the last two rows
                                        if (j != BaseVariableindex)
                                            panel.Controls.Add(new TextBox
                                            {
                                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                                TextAlign = HorizontalAlignment.Center,
                                                BorderStyle = BorderStyle.Fixed3D,
                                                BackColor = Color.WhiteSmoke,
                                                Width = 90,
                                                ReadOnly = true,
                                                Text = table[i][j].ToString("0.##") + "+(" + table[i + 1][j].ToString("0.##") + "M)",
                                            });
                                        // Highlight pivot column elements with light green
                                        else panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.LightGreen,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = table[i][j].ToString("0.##") + "+(" + table[i + 1][j].ToString("0.##") + "M)",
                                        });
                                    }
                                    else
                                    {
                                        // Display non-zero elements without 'M' for the last two rows
                                        if (j != BaseVariableindex)
                                            panel.Controls.Add(new TextBox
                                            {
                                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                                TextAlign = HorizontalAlignment.Center,
                                                BorderStyle = BorderStyle.Fixed3D,
                                                BackColor = Color.WhiteSmoke,
                                                Width = 90,
                                                ReadOnly = true,
                                                Text = table[i][j].ToString("0.##"),
                                            });
                                        // Highlight pivot column elements with light green
                                        else panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.LightGreen,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = table[i][j].ToString("0.##"),
                                        });
                                    }
                                }
                                else
                                {
                                    if (table[i + 1][j] != 0)
                                    {
                                        // Display 'M' elements for the last two rows
                                        if (j != BaseVariableindex)
                                            panel.Controls.Add(new TextBox
                                            {
                                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                                TextAlign = HorizontalAlignment.Center,
                                                BorderStyle = BorderStyle.Fixed3D,
                                                BackColor = Color.WhiteSmoke,
                                                Width = 90,
                                                ReadOnly = true,
                                                Text = "(" + table[i + 1][j].ToString("0.##") + "M)",
                                            });
                                        // Highlight pivot column elements with light green
                                        else panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.LightGreen,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = "(" + table[i + 1][j].ToString("0.##") + "M)",
                                        });
                                    }
                                    else
                                    {
                                        // Display zero elements for the last two rows
                                        if (j != BaseVariableindex)
                                            panel.Controls.Add(new TextBox
                                            {
                                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                                TextAlign = HorizontalAlignment.Center,
                                                BorderStyle = BorderStyle.Fixed3D,
                                                BackColor = Color.WhiteSmoke,
                                                Width = 90,
                                                ReadOnly = true,
                                                Text = table[i][j].ToString("0.##"),
                                            });
                                        // Highlight pivot column elements with light green
                                        else panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.LightGreen,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = table[i][j].ToString("0.##"),
                                        });
                                    }
                                }
                            }
                        }
                        NewLine(panel);
                    }
                }


                string S = allVars[BaseVariableindex] + " Enters the Basis, " + leftVars[UpToBaseVariableindex] + " Leaves From the Basis.";

                // Update the entering and leaving variables
                leftVars[UpToBaseVariableindex] = allVars[BaseVariableindex];
                // Add text to indicate the variable changes
                AddText(MainPanel, S, Color.Black);
                // Add empty line for spacing
                AddText(MainPanel, "", Color.White);

                // Store a copy of the current tableau for history
                List<List<double>> PrevTab = new List<List<double>>();

                foreach (List<double> innerList in table)
                {
                    List<double> innerCopy = new List<double>(innerList);
                    PrevTab.Add(innerCopy);
                }

                history.Add(PrevTab);
                // Retrieve the pivot element
                pivot = PrevTab[UpToBaseVariableindex][BaseVariableindex];
                if (pivot == 0) break;

                // Perform pivot operation to make pivot element 1 in its row
                for (int i = 0; i < table[UpToBaseVariableindex].Count; i++)
                {
                    table[UpToBaseVariableindex][i] = table[UpToBaseVariableindex][i] / pivot;
                }

                // Update other elements in the tableau
                for (int i = 0; i < table.Count; i++)
                {
                    for (int j = 0; j < table[UpToBaseVariableindex].Count; j++)
                    {
                        if (i != UpToBaseVariableindex)
                        {
                            if (!NeedAV())
                            {
                                // Update elements in the tableau for non-artificial variable case
                                double a = (double)PrevTab[i][j];
                                double b = (double)PrevTab[i][BaseVariableindex];
                                double c = (double)PrevTab[UpToBaseVariableindex][j];
                                double p = (double)pivot;
                                double R = a - ((b / p) * c);
                                table[i][j] = R;
                            }
                            else
                            {
                                // Update elements in the tableau for artificial variable case
                                if (i < table.Count - 2)
                                {
                                    double a = (double)PrevTab[i][j];
                                    double b = (double)PrevTab[i][BaseVariableindex];
                                    double c = (double)PrevTab[UpToBaseVariableindex][j];
                                    double p = (double)pivot;
                                    double R = a - ((b / p) * c);
                                    table[i][j] = R;
                                }
                                else
                                {
                                    double a = (double)PrevTab[i][j];
                                    double b = (double)PrevTab[i][BaseVariableindex];
                                    double c = (double)PrevTab[UpToBaseVariableindex][j];
                                    double p = (double)pivot;
                                    BigInteger R = (BigInteger)(a - ((b / p) * c));
                                    table[i][j] = (double)R;
                                }
                            }
                        }
                    }
                }
            }
            // Check if pivot is zero and if cycle is detected
            if (pivot != 0 && !Sycle)
            {
                // Display the updated tableau
                AddText(MainPanel, "- Iteration " + (iteration + 1) + ":", Color.Green);
                panel.Controls.Add(new TextBox
                {
                    Font = new Font("Arial", 12),
                    TextAlign = HorizontalAlignment.Center,
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.Snow,
                    Width = 90,
                    ReadOnly = true,
                    Enabled = false,
                });
                for (int i = 0; i < allVars.Count; i++)
                {
                    panel.Controls.Add(new TextBox
                    {
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        TextAlign = HorizontalAlignment.Center,
                        BorderStyle = BorderStyle.Fixed3D,
                        BackColor = Color.LightSteelBlue,
                        Width = 90,
                        ReadOnly = true,
                        Text = allVars[i],
                    });
                }
                NewLine(panel);
                if (!NeedAV())
                {
                    for (int i = 0; i < table.Count; i++)
                    {
                        if (i < leftVars.Count)
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Arial", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.LightSteelBlue,
                                Width = 90,
                                ReadOnly = true,
                                Text = leftVars[i],
                            });
                        else
                        {
                            if (Form1.isMax)
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Arial", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightSteelBlue,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = "Max Z"
                                });
                            else
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Arial", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightSteelBlue,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = "Min Z"
                                });
                        }

                        for (int j = 0; j < table[i].Count; j++)
                        {
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.Snow,
                                Width = 90,
                                ReadOnly = true,
                                Text = table[i][j].ToString("0.##"),
                            });
                        }
                        NewLine(panel);
                    }
                }
                else
                {
                    for (int i = 0; i < table.Count - 1; i++)
                    {
                        if (i < leftVars.Count)
                            panel.Controls.Add(new TextBox
                            {
                                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                TextAlign = HorizontalAlignment.Center,
                                BorderStyle = BorderStyle.Fixed3D,
                                BackColor = Color.LightSteelBlue,
                                Width = 90,
                                ReadOnly = true,
                                Text = leftVars[i],
                            });
                        else
                        {
                            if (Form1.isMax)
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightSteelBlue,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = "Max Z"
                                });
                            else
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.LightSteelBlue,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = "Min Z"
                                });
                        }

                        for (int j = 0; j < table[i].Count; j++)
                        {
                            if (i != table.Count - 2)
                            {
                                panel.Controls.Add(new TextBox
                                {
                                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                    TextAlign = HorizontalAlignment.Center,
                                    BorderStyle = BorderStyle.Fixed3D,
                                    BackColor = Color.Snow,
                                    Width = 90,
                                    ReadOnly = true,
                                    Text = table[i][j].ToString("0.##"),
                                });
                            }
                            else
                            {
                                if (table[i][j] != 0)
                                {
                                    if (table[i + 1][j] != 0)
                                    {
                                        panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.Snow,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = table[i][j].ToString("0.##") + "+(" + table[i + 1][j].ToString("0.##") + "M)",
                                        });
                                    }
                                    else
                                    {
                                        panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.Snow,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = table[i][j].ToString("0.##"),
                                        });
                                    }
                                }
                                else
                                {
                                    if (table[i + 1][j] != 0)
                                    {
                                        panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.Snow,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = "(" + table[i + 1][j].ToString("0.##") + "M)",
                                        });
                                    }
                                    else
                                    {
                                        panel.Controls.Add(new TextBox
                                        {
                                            Font = new Font("Segoe UI", 12, FontStyle.Bold),
                                            TextAlign = HorizontalAlignment.Center,
                                            BorderStyle = BorderStyle.Fixed3D,
                                            BackColor = Color.Snow,
                                            Width = 90,
                                            ReadOnly = true,
                                            Text = table[i][j].ToString("0.##"),
                                        });
                                    }
                                }
                            }
                        }
                        NewLine(panel);
                    }
                }

                AddText(MainPanel, "", Color.Black);
                AddText(MainPanel, "● The Optimal Solution is Found:", Color.FromArgb(0, 46, 119));
                /* for (int i = 0; i < leftVars.Count; i++)
                 {
                     string Text = "○ " + leftVars[i] + " = " + table[i].Last().ToString("0.##");
                     AddText(MainPanel, Text, Color.Black);
                 }*/
                string Z;
                if (!NeedAV())
                    Z = "○ Z = " + Math.Abs(table.Last().Last());
                else
                    Z = "○ Z = " + Math.Abs(table[table.Count - 2].Last()).ToString("0.##");
                AddText(MainPanel, Z, Color.Black);
                AddText(MainPanel, "", Color.Black);
            }
            else if (pivot == 0)
            // Display message indicating no optimal solution due to unboundedness
            {
                AddText(MainPanel, "● No Optimal Solution can be Found:", Color.FromArgb(0, 46, 119));

                AddText(MainPanel, "Encountering a pivot value of zero in the simplex method might indicate unboundedness. ", Color.Black);
                AddText(MainPanel, "This means that, in certain situations, the problem has no finite maximum or minimum value for the objective function.", Color.Black);
                AddText(MainPanel, "When a pivot value becomes zero during pivoting, it suggests that the current constraints and objective function ", Color.Black);
                AddText(MainPanel, "allow the solution to extend infinitely in at least one direction. In practical terms, this implies that the problem ", Color.Black);
                AddText(MainPanel, "doesn't have a bounded solution within the given constraints. Hence, the simplex method cannot find an optimal solution ", Color.Black);
                AddText(MainPanel, "as there's no finite best value for the objective function.", Color.Black);
            }
            else
            // Display message indicating cycle detection
            {
                AddText(MainPanel, "● No Optimal Solution can be Found:", Color.FromArgb(0, 46, 119));

                AddText(MainPanel, "Cycle Detected: The simplex method has encountered a cycle in the iteration process. This means that the algorithm is ", Color.Black);
                AddText(MainPanel, "revisiting previously explored solutions without progressing towards the optimal solution ", Color.Black);
                AddText(MainPanel, "or determining unboundedness.", Color.Black);
            }
        }




        // Method to find the index of the maximum value in a list
        int maxIndex(List<double> l)
        {
            if (Form1.isMax)
            {
                double maxPositive = double.MinValue;
                int index = 0;

                for (int i = 0; i < ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1; i++)
                {
                    if (l[i] > 0 && l[i] > maxPositive)
                    {
                        maxPositive = l[i];
                        index = i;
                    }
                }
                return index;
            }
            else
            {
                double minNegative = double.MaxValue;
                int index = 0;

                for (int i = 0; i < ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1; i++)
                {
                    if (l[i] < 0 && l[i] < minNegative)
                    {
                        minNegative = l[i];
                        index = i;
                    }
                }

                return index;
            }
        }

        // Method to find the index of the minimum left values in a list
        int minLeftValuesIndex(List<List<double>> l, int BaseVariableindex)
        {
            int index = 0;
            double value = double.MaxValue;

            for (int i = 0; i < Form1.ProblemCons.Count; i++)
            {
                if (l[i].Last() / l[i][BaseVariableindex] > 0 && l[i].Last() / l[i][BaseVariableindex] < value)
                {
                    value = l[i].Last() / l[i][BaseVariableindex];
                    index = i;
                }
            }
            return index;
        }

        // Method to check stopping conditions for the simplex algorithm
        bool Stop(List<List<double>> l)
        {
            if (Form1.isMax)
            {
                if (!NeedAV())
                {
                    for (int i = 0; i < ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1; i++)
                    {
                        if (l.Last()[i] > 0) return false;
                    }
                    return true;
                }
                else
                {
                    List<double> lastList = l[l.Count - 1];
                    List<double> secondLastList = l[l.Count - 2];


                    var lastNElementsLastList = lastList.Take(ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1);
                    var lastNElementsSecondLastList = secondLastList.Take(ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1);

                    bool areAllElementsInLastListNegative = lastNElementsLastList.All(x => x <= 0);
                    bool areAllElementsInSecondLastListNegative = lastNElementsSecondLastList.All(x => x <= 0);

                    return areAllElementsInLastListNegative && areAllElementsInSecondLastListNegative;
                }
            }
            else
            {
                if (!NeedAV())
                {
                    for (int i = 0; i < ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1; i++)
                    {
                        if (l.Last()[i] < 0) return false;
                    }
                    return true;
                }
                else
                {
                    List<double> lastList = l[l.Count - 1];
                    List<double> secondLastList = l[l.Count - 2];

                    var lastNElementsLastList = lastList.Take(ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1);
                    var lastNElementsSecondLastList = secondLastList.Take(ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1);

                    bool areAllElementsInLastListNonNegative = lastNElementsLastList.All(x => x >= 0);
                    bool areAllElementsInSecondLastListNonNegative = lastNElementsSecondLastList.All(x => x >= 0);

                    return areAllElementsInLastListNonNegative && areAllElementsInSecondLastListNonNegative;
                }
            }
        }

        // Method to check if all values in a list are equal
        bool AllValuesEqual(List<double> list)
        {
            if (list.Count < ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1)
            {
                return false;
            }

            var firstNElements = list.Take(ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count - 1);
            bool areEqual = firstNElements.All(x => x == firstNElements.First());
            return areEqual;
        }

        // Method to check if a list contains non-negative values
        bool HasNonNegative(List<double> numbers)
        {
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] > 0)
                {
                    return true;
                }
            }

            return false;
        }

        // Method to check if a list contains non-positive values
        bool HasNonPsitive(List<double> numbers)
        {
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] < 0)
                {
                    return true;
                }
            }

            return false;
        }

        // Method to check if a table exists in history
        bool ExistsBefore(List<List<List<double>>> history, List<List<double>> table)
        {
            foreach (var entry in history)
            {
                bool isEqual = true;

                if (entry.Count != table.Count)
                    continue;

                for (int i = 0; i < entry.Count; i++)
                {
                    if (!entry[i].SequenceEqual(table[i]))
                    {
                        isEqual = false;
                        break;
                    }
                }

                if (isEqual)
                    return true;
            }
            return false;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            MainPanel.AutoScroll = true;
            MainPanel.AutoScrollMinSize = new Size(90 * (ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count + 2) + (3 * ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection).Count + 1), 0);


            // Display the given linear programming problem
            AddText(MainPanel, "● The Given Linear Programming Problem:", Color.FromArgb(0, 46, 119));
            ShowTheProblem(MainPanel);

            // Display the constraints of the problem
            AddText(MainPanel, "Subject to the Constraints:", Color.Black);
            ShowConstraints(MainPanel);
            AddText(MainPanel, "", Color.Black);

            // Display the definition of the standard form
            AddText(MainPanel, "● Definition of Standard Form:", Color.FromArgb(0, 46, 119));
            ShowStandardForm(MainPanel);
            AddText(MainPanel, "", Color.Black);

            // Display the construction of the first table
            AddText(MainPanel, "● Construction of the First Table:", Color.FromArgb(0, 46, 119));
            ShowFirstTab(
                MainPanel,
                ReturnTheNewTable(Form1.ProblemCons, Form1.ProblemDirection, Form1.RightVal),
                ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection),
                ReturnLeftVars(Form1.ProblemDirection));

            // Display the iterations of the simplex algorithm
            AddText(MainPanel, "", Color.Black);
            AddText(MainPanel, "● Iterations of Simplex Algorithm:", Color.FromArgb(0, 46, 119));
            SimplexIterations(
                MainPanel,
                ReturnTheNewTable(Form1.ProblemCons, Form1.ProblemDirection, Form1.RightVal),
                ReturnAllVariables(Form1.ProblemCons, Form1.ProblemDirection),
                ReturnLeftVars(Form1.ProblemDirection)
                );
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Clear the lists in Form1 to reset the problem data
            Form1.ProblemCons.Clear();
            Form1.ProblemVar.Clear();
            Form1.ProblemDirection.Clear();
            Form1.RightVal.Clear();
        }

        private void MainPanel_Paint(object sender, PaintEventArgs e)
        {
            // Set the background color to a lighter shade of blue
            Color lighterBlue = Color.FromArgb(173, 216, 230); // Adjust the RGB values for the desired shade
            e.Graphics.Clear(lighterBlue);

            // You can also draw other elements or perform custom painting here if needed
        }
    }
}
