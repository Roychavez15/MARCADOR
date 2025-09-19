using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace MARCADORMANUAL
{
    public partial class Form1 : Form
    {
        string connectionString = "";
        private static cIniArray mINI = new cIniArray();
        bool comerciales = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string str = mINI.IniGet(Application.StartupPath + @"\Config.ini", "Bases", "Servidor", "");
            string str2 = mINI.IniGet(Application.StartupPath + @"\Config.ini", "Bases", "Base", "");
            string str3 = mINI.IniGet(Application.StartupPath + @"\Config.ini", "Bases", "Usuario", "");
            string str4 = mINI.IniGet(Application.StartupPath + @"\Config.ini", "Bases", "Clave", "");

            
            connectionString = (((((connectionString + "workstation id=SISTEMAS6;" + "packet size=4096;") + "user id=" + str3 + ";") + "data source=\"" + str + "\";") + "persist security info=True;") + "initial catalog=" + str2 + ";") + "password=" + str4;


            //for (int i = 0; i < 100;i++ )
            //{
            //    comboBox3.Items.Add(i.ToString());
            //    comboBox4.Items.Add(i.ToString());
            //}
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            Campeonato();
            Equipos();
            Inicia();
        }
        private void Campeonato()
        {
            label3.Text = DateTime.Now.Year.ToString("0000");


            SqlConnection con = new SqlConnection(connectionString);
            string sql = "SELECT TOP 1 CAMPEONATO FROM CONFIGURACIONES ORDER BY CAMPEONATO DESC";

            SqlCommand obj = new SqlCommand(sql, con);
            obj.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = obj.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    label3.Text = dr[0].ToString().Trim();

                }

            }
            else
            {

            }
            dr.Close();
            con.Close();
            con.Dispose();
            dr.Dispose();

        }
        private void Equipos()
        {
            


            SqlConnection con = new SqlConnection(connectionString);
            string sql = "SELECT distinct(nombre) FROM EquiposCampeonato";
            sql+=" where Campeonato ="+label3.Text ;
            sql+=" order by Nombre";

            SqlCommand obj = new SqlCommand(sql, con);
            obj.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = obj.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    comboBox1.Items.Add (dr[0].ToString().Trim());
                    comboBox2.Items.Add (dr[0].ToString().Trim());
                }

            }
            else
            {

            }
            dr.Close();
            con.Close();
            con.Dispose();
            dr.Dispose();

        }
        private void Inicia()
        {
            ////LOCAL
            String line;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\LOCAL.txt");

                //Read the first line of text
                line = sr.ReadLine();
                label5.Text = line;
                comboBox3.Text = line;

                sr.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }


            ////VISITANTE
            String line1;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\VISITANTE.txt");

                //Read the first line of text
                line1 = sr.ReadLine();
                label6.Text = line1;
                comboBox4.Text = line1;

                sr.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }

            ////EQUIPO LOCAL
            String line2;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\ELOCAL.txt");

                //Read the first line of text
                line2 = sr.ReadLine();

                comboBox1.Text = line2;

                sr.Close();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }

            ////EQUIPO VISITANTE
            String line3;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\EVISITANTE.txt");

                //Read the first line of text
                line3 = sr.ReadLine();
                comboBox2.Text = line3;

                sr.Close();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }


            String line5;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\MENSAJE.txt");

                //Read the first line of text
                line5 = sr.ReadLine();
                textBox1.Text  = line5;
                sr.Close();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }

            GolLocal();
            GolVisitante();
        }

        private void NumerosLocal()
        {
            comboBox3.Items.Clear();
            comboBox3.Items.Add("0");

            SqlConnection con = new SqlConnection(connectionString);
            string sql = "SELECT numero from jugadorescampeonato where ";
            sql += " equipo=(select id from equipos where nombrecorto='" + comboBox1.Text + "') and tipo='J'";
            sql += " AND CAMPEONATO=" + label3.Text;
            sql += " ORDER BY NUMERO ASC";

            SqlCommand obj = new SqlCommand(sql, con);
            obj.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = obj.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    //label3.Text = dr[0].ToString().Trim();
                    comboBox3.Items.Add(dr[0].ToString().Trim());
                }

            }
            else
            {

            }
            dr.Close();
            con.Close();
            con.Dispose();
            dr.Dispose();
            comboBox3.Text = "0";
        }

        private void NumerosVisitante()
        {
            comboBox4.Items.Clear();
            comboBox4.Items.Add("0");
            SqlConnection con = new SqlConnection(connectionString);
            string sql = "SELECT numero from jugadorescampeonato where ";
            sql += " equipo=(select id from equipos where nombrecorto='" + comboBox2.Text + "') and tipo='J'";
            sql += " AND CAMPEONATO=" + label3.Text;
            sql += " ORDER BY NUMERO ASC";

            SqlCommand obj = new SqlCommand(sql, con);
            obj.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = obj.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    //label3.Text = dr[0].ToString().Trim();
                    comboBox4.Items.Add(dr[0].ToString().Trim());
                }

            }
            else
            {

            }
            dr.Close();
            con.Close();
            con.Dispose();
            dr.Dispose();
            comboBox4.Text = "0";
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(Escribe("LOCAL",comboBox3.Text))
            //{
            //    label5.Text = comboBox3.Text;
            //}
            if (comboBox3.Text != "0")
            {
                string Nombre= Jugador(comboBox3.Text, comboBox1.Text);

                DialogResult result = MessageBox.Show("Desea Ingresar el Gol del jugador N: " +comboBox3.Text + " " +Nombre +"?", "Confirmacion", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    //...
                    bool salida = Score(comboBox3.Text, comboBox1.Text);
                    if (salida)
                    {
                        OCULTA();
                        System.Threading.Thread.Sleep(2000);
                        Gol();
                        GolLocal();
                        timer2.Start();
                    }
                    
                }
                else if (result == DialogResult.No)
                {
                    //...
                }
                else
                {
                    //...
                }

                comboBox3.Text = "0";
            }
        }
        private string Jugador(string Numero, string Equipo)
        {
            string nombre = "";
            SqlConnection con = new SqlConnection(connectionString);
            string sql = "SELECT nombres from jugadorescampeonato where ";
            sql += " equipo=(select id from equipos where nombrecorto='" + Equipo + "') and tipo='J'";
            sql += " AND CAMPEONATO=" + label3.Text+" and numero="+Numero ;
            sql += " ORDER BY NUMERO ASC";

            SqlCommand obj = new SqlCommand(sql, con);
            obj.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = obj.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    nombre = dr[0].ToString().Trim();
                    //comboBox4.Items.Add(dr[0].ToString().Trim());
                }

            }
            else
            {

            }
            dr.Close();
            con.Close();
            con.Dispose();
            dr.Dispose();



            return nombre;
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Escribe("VISITANTE", comboBox4.Text))
            //{
            //    label6.Text = comboBox4.Text;
            //}

            if (comboBox4.Text != "0")
            {
                string Nombre = Jugador(comboBox4.Text, comboBox2.Text);

                DialogResult result = MessageBox.Show("Desea Ingresar el Gol del jugador N: " +comboBox4.Text + " " + Nombre + "?", "Confirmacion", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    //...
                    bool salida = Score(comboBox4.Text, comboBox2.Text);
                    if (salida)
                    {
                        OCULTA();
                        System.Threading.Thread.Sleep(2000);
                        Gol();
                        GolVisitante();
                        timer2.Start();
                    }
                }
                else if (result == DialogResult.No)
                {
                    //...
                }
                else
                {
                    //...
                }

                comboBox4.Text = "0";
            }
        }
        private bool  Escribe(string equipo, string valor)
        {
            bool salida = false;
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath +@"\"+equipo+".txt");

                //Write a line of text
                sw.Write(valor);
                //Close the file
                sw.Close();
                salida = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                salida = false;
            }
            finally
            {
                //MessageBox.Show("Executing finally block.");
                salida = true;
            }
            return salida;
        }
        private bool EscribeEquipos(string equipo, string valor)
        {
            bool salida = false;
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\E" + equipo + ".txt");

                //Write a line of text
                sw.Write(valor);
                //Close the file
                sw.Close();
                salida = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                salida = false;
            }
            finally
            {
                //MessageBox.Show("Executing finally block.");
                salida = true;
            }
            return salida;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
                
            //    SqlConnection connection1 = new SqlConnection(connectionString);
            //    string str6 = "DELETE SCORE WHERE EQUIPO='" + comboBox1.Text + "'";

            //    SqlCommand command1 = new SqlCommand(str6, connection1)
            //    {
            //        CommandType = CommandType.Text
            //    };
            //    connection1.Open();
            //    command1.ExecuteNonQuery();

            //}
            //catch { }

            if(EscribeEquipos ("LOCAL",comboBox1.Text ))
            {
                //label3.Text = comboBox1.Text;
                groupBox4.Text = comboBox1.Text;
            }
            NumerosLocal();
            GolLocal();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{

            //    SqlConnection connection1 = new SqlConnection(connectionString);
            //    string str6 = "DELETE SCORE WHERE EQUIPO='" + comboBox2.Text + "'";

            //    SqlCommand command1 = new SqlCommand(str6, connection1)
            //    {
            //        CommandType = CommandType.Text
            //    };
            //    connection1.Open();
            //    command1.ExecuteNonQuery();

            //}
            //catch { }

            if (EscribeEquipos("VISITANTE", comboBox2.Text))
            {
                //label4.Text = comboBox2.Text;
                groupBox5.Text = comboBox2.Text;
            }
            NumerosVisitante();
            GolVisitante();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\MENSAJE.txt");

                //Write a line of text
                sw.Write(textBox1.Text.ToUpper ());
                //Close the file
                sw.Close();

            }
            catch (Exception eX)
            {
                MessageBox.Show(eX.Message);
            }
            finally
            {

            }
        }

        private bool EscribeTiempo(string valor)
        {
            bool salida = false;
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\tiempo.txt");

                //Write a line of text
                sw.Write(valor);
                //Close the file
                sw.Close();
                salida = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                salida = false;
            }
            finally
            {
                //MessageBox.Show("Executing finally block.");
                salida = true;
            }
            return salida;
        }
        private bool EscribeEtapa(string valor)
        {
            bool salida = false;
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\Etapa.txt");

                //Write a line of text
                sw.Write(valor);
                //Close the file
                sw.Close();
                salida = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                salida = false;
            }
            finally
            {
                //MessageBox.Show("Executing finally block.");
                salida = true;
            }
            return salida;
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EscribeTiempo (comboBox5.Text))
            {
                //label3.Text = comboBox1.Text;
            }
        }

        private bool Cronometro()
        {
            bool salida = false;
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\cronometro.txt");

                //Write a line of text
                sw.Write(DateTime.Now.ToString ("HH:mm:ss"));
                //Close the file
                sw.Close();
                salida = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                salida = false;
            }
            finally
            {
                //MessageBox.Show("Executing finally block.");
                salida = true;
            }


            return salida;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Esta Seguro de resetear el tiempo (00:00)", "Resetear Tiempo", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {

                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(Application.StartupPath + @"\OPCIONTIEMPO.txt");

                    //Write a line of text
                    sw.Write("1");
                    //Close the file
                    sw.Close();

                }
                catch (Exception eX)
                {

                }
                finally
                {

                }

                Cronometro();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String line4 = "0";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\OPCIONTIEMPO.txt");

                //Read the first line of text
                line4 = sr.ReadLine();

                sr.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }
            if (line4 == "1")
            {
                String line3;
                try
                {
                    //Pass the file path and file name to the StreamReader constructor
                    StreamReader sr = new StreamReader(Application.StartupPath + "\\CRONOMETRO.txt");

                    //Read the first line of text
                    line3 = sr.ReadLine();
                    DateTime TIEMPOACTUAL = DateTime.Now;
                    DateTime TIEMPOINICIO = Convert.ToDateTime(line3);

                    TimeSpan total = new TimeSpan(TIEMPOACTUAL.Ticks - TIEMPOINICIO.Ticks);
                    //DateTime TIEMPO = TIEMPOACTUAL - TIEMPOINICIO;

                    label9.Text = total.Minutes.ToString("00") + ":" + total.Seconds.ToString("00");
                    if (total.Minutes == 15 || total.Minutes == 30)
                    {
                        System.Media.SystemSounds.Beep.Play();
                        button10.BackColor = Color.Red;
                        button10.Text = "COLOCAR COMERCIALES !!!!!!";
                    }
                    else
                    {
                        button10.BackColor = Color.Silver ;
                        button10.Text = "MOSTRAR MARCADOR";
                    }
                    sr.Close();
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Exception: " + ex.Message);
                }
                finally
                {
                    //Console.WriteLine("Executing finally block.");
                }
            }
            else
            {
                label9.Text = "00:00";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\OPCIONTIEMPO.txt");

                //Write a line of text
                sw.Write("2");
                //Close the file
                sw.Close();

            }
            catch (Exception eX)
            {

            }
            finally
            {

            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            string ocultar = "0";
            if (checkBox2.Checked == true)
            {
                ocultar = "1";
            }
            else
            {
                ocultar = "0";
            }

            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\OCULTAR.txt");

                //Write a line of text
                sw.Write(ocultar);
                //Close the file
                sw.Close();

            }
            catch (Exception eX)
            {

            }
            finally
            {

            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            EscribeEtapa(comboBox6.Text);
        }

        private void Gol()
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\gol.txt");

                //Write a line of text
                sw.Write("1");
                //Close the file
                sw.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

            }
            finally
            {
                //MessageBox.Show("Executing finally block.");

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Gol();
            timer2.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Gol();
            timer2.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\gol.txt");

                //Write a line of text
                sw.Write("0");
                //Close the file
                sw.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //MessageBox.Show("Executing finally block.");

            }
        }
        private void OCULTA()
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\gol.txt");

                //Write a line of text
                sw.Write("0");
                //Close the file
                sw.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //MessageBox.Show("Executing finally block.");

            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\gol.txt");

                //Write a line of text
                sw.Write("0");
                //Close the file
                sw.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //MessageBox.Show("Executing finally block.");
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {

                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(Application.StartupPath + @"\gol.txt");

                //Write a line of text
                sw.Write("0");
                //Close the file
                sw.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                //MessageBox.Show("Executing finally block.");

            }

            timer2.Stop();
        }

        private bool Score(string numero, string equipo)
        {
            bool salida = false;
            try
            {
                //////registra MAIL
                SqlConnection connection1 = new SqlConnection(connectionString);
                string str6 = "INSERT INTO SCORE (NUMERO,TIEMPO,EQUIPO) VALUES (";
                str6+=numero +","+label9.Text.Substring (0,2)+",'"+equipo +"')";

                SqlCommand command1 = new SqlCommand(str6, connection1)
                {
                    CommandType = CommandType.Text
                };
                connection1.Open();
                command1.ExecuteNonQuery();
                salida = true;
                ////////////////////////
            }
            catch { salida = false; }

            return salida;
        }
        private void GolLocal()
        {
            SqlConnection con = new SqlConnection(connectionString);

            string sql = "";

            sql = "SELECT NUMERO AS N, TIEMPO FROM SCORE WHERE EQUIPO='" + comboBox1.Text + "'";
            sql += " order by id desc";


            SqlCommand obj = new SqlCommand(sql, con);
            obj.CommandType = CommandType.Text;
            con.Open();

            DataSet dset = new DataSet();
            SqlDataAdapter dadapt = new SqlDataAdapter(sql, con);
            dadapt.Fill(dset, "GL");
            dataGridView1.DataSource = dset.Tables["GL"];
            dataGridView1.Columns[0].Width = 50;
            Escribe("LOCAL", dset.Tables["GL"].Rows.Count.ToString());
            label5.Text = dset.Tables["GL"].Rows.Count.ToString();
            con.Close();
            con.Dispose();
        }
        private void GolVisitante()
        {
            SqlConnection con = new SqlConnection(connectionString);

            string sql = "";

            sql = "SELECT NUMERO AS N, TIEMPO FROM SCORE WHERE EQUIPO='" + comboBox2.Text + "'";
            sql += " order by id desc";


            SqlCommand obj = new SqlCommand(sql, con);
            obj.CommandType = CommandType.Text;
            con.Open();

            DataSet dset = new DataSet();
            SqlDataAdapter dadapt = new SqlDataAdapter(sql, con);
            dadapt.Fill(dset, "GV");
            dataGridView2.DataSource = dset.Tables["GV"];
            dataGridView2.Columns[0].Width = 50;

            Escribe("VISITANTE", dset.Tables["GV"].Rows.Count.ToString());
            label6.Text = dset.Tables["GV"].Rows.Count.ToString();
            con.Close();
            con.Dispose();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label4.Text = dataGridView2[0, e.RowIndex].Value.ToString ();
            label11.Text = dataGridView2[1, e.RowIndex].Value.ToString();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            label12.Text = dataGridView1[0, e.RowIndex].Value.ToString();
            label13.Text = dataGridView1[1, e.RowIndex].Value.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
                        DialogResult dialogResult = MessageBox.Show("Esta Seguro de Borrar este gol del numero "+label12.Text +" en el tiempo= "+label13.Text , "Quitar Gol", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            try
                            {
                                //////registra MAIL
                                SqlConnection connection1 = new SqlConnection(connectionString);
                                string str6 = "delete score where equipo='" + comboBox1.Text + "'";
                                str6 += " AND NUMERO=" + label12.Text + " AND TIEMPO=" + label13.Text;

                                SqlCommand command1 = new SqlCommand(str6, connection1)
                                {
                                    CommandType = CommandType.Text
                                };
                                connection1.Open();
                                command1.ExecuteNonQuery();
                                GolLocal();
                                label12.Text = "0";
                                label13.Text = "0";
                                ////////////////////////
                            }
                            catch { }
                        }
        }

        private void button8_Click(object sender, EventArgs e)
        {
                                    DialogResult dialogResult = MessageBox.Show("Esta Seguro de Borrar este gol del numero "+label4.Text +" en el tiempo= "+label11.Text , "Quitar Gol", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        try
                                        {
                                            //////registra MAIL
                                            SqlConnection connection1 = new SqlConnection(connectionString);
                                            string str6 = "delete score where equipo='" + comboBox2.Text + "'";
                                            str6 += " AND NUMERO=" + label4.Text + " AND TIEMPO=" + label11.Text;

                                            SqlCommand command1 = new SqlCommand(str6, connection1)
                                            {
                                                CommandType = CommandType.Text
                                            };
                                            connection1.Open();
                                            command1.ExecuteNonQuery();
                                            GolVisitante();
                                            label4.Text = "0";
                                            label11.Text = "0";
                                            ////////////////////////
                                        }
                                        catch { }
                                    }
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }
    }
}
