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

namespace MARCADOR
{
    public partial class Form1 : Form
    {
        Point p;

        int intSalida;

        int intDiferencia;
        int contadorp = 0;
        //factor de velocidad de desplazamiento

        decimal velocidad = 0.05m;

        public enum Direccion
        {

            izquierda,

            derecha

        }
        Direccion d;

        int contador = 0;
        private static cIniArray mINI = new cIniArray();
        string connectionString = "";
        public Form1()
        {
            InitializeComponent();

            p = label1.Location;

            d = Direccion.derecha;

            intSalida = System.Environment.TickCount;

            label1.Location = new Point(192, 170);

            panel1.Location = new Point(0, 0);
            contadorp = 0;

            
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Funcion();
        }
        private void Funcion()
        {
            ////LOCAL
            String line;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\LOCAL.txt");

                //Read the first line of text
                line = sr.ReadLine();
                try
                {
                    int valorlocal = Convert.ToInt32(line);
                    if (valorlocal < 10)
                    {
                        pictureBox5.Visible = false;
                        pictureBox6.Location = new Point(30, 105);
                        pictureBox6.Image = Image.FromFile(Application.StartupPath.ToString() + @"\NUMEROS\" + valorlocal.ToString() + ".jpg");

                    }
                    else
                    {

                        pictureBox5.Visible = true;
                        pictureBox5.Location = new Point(12, 105);
                        pictureBox6.Location = new Point(43, 105);

                        pictureBox5.Image = Image.FromFile(Application.StartupPath.ToString() + @"\NUMEROS\" + valorlocal.ToString().Substring(0, 1) + ".jpg");
                        pictureBox6.Image = Image.FromFile(Application.StartupPath.ToString() + @"\NUMEROS\" + valorlocal.ToString().Substring(1, 1) + ".jpg");
                    }
                }

                catch { }

                ////Continue to read until you reach end of file
                //while (line != null)
                //{
                //    //write the lie to console window
                //    Console.WriteLine(line);
                //    //Read the next line
                //    line = sr.ReadLine();
                //}

                //close the file
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
                try
                {
                    int valorvisitante = Convert.ToInt32(line1);
                    if (valorvisitante < 10)
                    {
                        pictureBox8.Visible = false;
                        pictureBox7.Location = new Point(135, 105);
                        pictureBox7.Image = Image.FromFile(Application.StartupPath.ToString() + @"\NUMEROS\" + valorvisitante.ToString() + ".jpg");
                    }
                    else
                    {
                        pictureBox8.Visible = true;
                        pictureBox8.Location = new Point(118, 105);
                        pictureBox7.Location = new Point(150, 105);
                        pictureBox8.Image = Image.FromFile(Application.StartupPath.ToString() + @"\NUMEROS\" + valorvisitante.ToString().Substring(0, 1) + ".jpg");
                        pictureBox7.Image = Image.FromFile(Application.StartupPath.ToString() + @"\NUMEROS\" + valorvisitante.ToString().Substring(1, 1) + ".jpg");
                    }
                }

                catch { }

                ////Continue to read until you reach end of file
                //while (line != null)
                //{
                //    //write the lie to console window
                //    Console.WriteLine(line);
                //    //Read the next line
                //    line = sr.ReadLine();
                //}

                //close the file
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
                try
                {
                    pictureBox3.Image = Image.FromFile(Application.StartupPath.ToString() + @"\EQUIPOS\" + line2 + ".jpg");
                }
                catch { pictureBox3.Image = null; }
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

            ////EQUIPO VISITANTE
            String line3;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\EVISITANTE.txt");

                //Read the first line of text
                line3 = sr.ReadLine();
                try
                {
                    pictureBox4.Image = Image.FromFile(Application.StartupPath.ToString() + @"\EQUIPOS\" + line3 + ".jpg");
                }
                catch { pictureBox4.Image = null; }
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

            ////EQUIPO tiempo
            String line4;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\tiempo.txt");

                //Read the first line of text
                line4 = sr.ReadLine();

                label4.Text = line4;

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


            ////etapa
            String line7="";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\Etapa.txt");

                //Read the first line of text
                line7 = sr.ReadLine();

                label2.Text = line7;

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
        }
        private void timer2_Tick(object sender, EventArgs e)
        {


            /////////////////////
            //intDiferencia = System.Environment.TickCount - intSalida;

            ////Calculo de deplazamiento de acuerdo al tiempo transcurrido

            ////Se utiliza el tickcount por precision y facilidad, pero podrias

            ////usar e valor de tempotrizacion del timer o usar un Stoptimer de mas

            ////presicion, pero para lo que necesitas

            //if (label1.Location.X + label1.Size.Width > this.Size.Width)
            //    d = Direccion.izquierda;
            //else if (label1.Location.X < 0)
            //    d = Direccion.derecha;
            //if (d == Direccion.derecha)
            //    p.X += (int)Math.Round(velocidad * intDiferencia);
            //else
            //    p.X -= (int)Math.Round(velocidad * intDiferencia);


            //label1.Location = p;
            //intSalida = System.Environment.TickCount;

            int termina = label1.Width * (-1);
            try
            {
                if (label1.Location.X <= termina)
                {
                    label1.Location = new Point(192, 170);
                }
                else
                {
                    p.X = label1.Location.X - 5;
                    label1.Location = p;

                }
            }
            catch { }

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            String line3;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\MENSAJE.txt");

                //Read the first line of text
                line3 = sr.ReadLine();
                label1.Text = line3;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string str = mINI.IniGet(Application.StartupPath + @"\Config.ini", "Bases", "Servidor", "");
            string str2 = mINI.IniGet(Application.StartupPath + @"\Config.ini", "Bases", "Base", "");
            string str3 = mINI.IniGet(Application.StartupPath + @"\Config.ini", "Bases", "Usuario", "");
            string str4 = mINI.IniGet(Application.StartupPath + @"\Config.ini", "Bases", "Clave", "");


            connectionString = (((((connectionString + "workstation id=SISTEMAS6;" + "packet size=4096;") + "user id=" + str3 + ";") + "data source=\"" + str + "\";") + "persist security info=True;") + "initial catalog=" + str2 + ";") + "password=" + str4;


            pictureBox9.Image = Image.FromFile(Application.StartupPath.ToString() + @"\gol.jpg");
            Campeonato();

            Funcion();
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //label5.Text = DateTime.Now.ToString("HH:mm");
            //

            String line7 = "0";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\OCULTAR.txt");

                //Read the first line of text
                line7 = sr.ReadLine();

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
            ///oculta tiempo
            if (line7 == "0")
            {
                label5.Visible = true;
            }
            else
            {
                label5.Visible = false;
            }

            String line4="0";
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

                    label5.Text = total.Minutes.ToString("00") + ":" + total.Seconds.ToString("00");
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
                label5.Text = "00:00";
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            //pictureBox9.Visible = false;
            //timer6.Stop();
            String line41 = "0";
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Application.StartupPath + "\\gol.txt");

                //Read the first line of text
                line41 = sr.ReadLine();
                sr.Close();
                if (line41.Trim () == "1")
                {
                    PRESENTAGOL();
                    panel1.Visible = true;
                    if (contadorp == 0)
                    {
                        
                        pictureBox9.Visible = true;
                    }
                    else
                    {
                        pictureBox9.Visible = false;
                    }
                    timer6.Start();
                    //System.Threading.Thread.Sleep(3000);
                    //pictureBox9.Visible = false;
                    //panel1.Visible = false;
                }
                else
                {
                    panel1.Visible = false;
                    pictureBox9.Visible = false;
                    timer6.Stop();
                    contadorp = 0;
                }
                
            }
            catch (Exception ex)
            {
                panel1.Visible = false;
                pictureBox9.Visible = false;
                timer6.Stop();
                MessageBox.Show(ex.Message);
                contadorp = 0;
                //Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                //panel1.Visible = false;
                //Console.WriteLine("Executing finally block.");
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            //pictureBox9.Visible = false;
            contadorp += 1;
            timer6.Stop();
        }
        private void PRESENTAGOL()
        {
            SqlConnection con = new SqlConnection(connectionString);
            string sql = "SELECT top 1 numero, equipo from score";
            sql += " ORDER BY id desc";

            SqlCommand obj = new SqlCommand(sql, con);
            obj.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = obj.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                SqlConnection con1 = new SqlConnection(connectionString);
                string sql1 = "SELECT NOMBRES, IDENTIFICACION";
                sql1 += " , (SELECT FOTO FROM JUGADORES WHERE JUGADORES.IDENTIFICACION=JUGADORESCAMPEONATO.IDENTIFICACION) AS FOTO";
                sql1 += " , (SELECT SUM(GOLES) FROM JUGADOS WHERE JUGADOS.IDENTIFICACION=JUGADORESCAMPEONATO.IDENTIFICACION AND JUGADOS.CAMPEONATO="+label12.Text +") AS GOLES";

                sql1 += " FROM JUGADORESCAMPEONATO WHERE ";
                sql1 += " numero=" + dr[0].ToString().Trim() + " and equipo=(select id from equipos where nombrecorto='" + dr[1].ToString().Trim() + "')";
                sql1 += " and campeonato=" + label12.Text;

                SqlCommand obj1 = new SqlCommand(sql1, con1);
                obj1.CommandType = CommandType.Text;
                con1.Open();

                SqlDataReader dr1= obj1.ExecuteReader();
                if (dr1.HasRows)
                {
                    dr1.Read();
                    label10.Text = dr1[0].ToString().Trim();
                    label11.Text = dr[0].ToString().Trim();
                    label6.Text = dr1[3].ToString().Trim();
                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(dr1[2]);
                        MemoryStream mem = new MemoryStream(data);
                        pictureBox2.Image = Image.FromStream(mem);
                    }
                    catch { pictureBox2.Image = null; }
                }
                else
                {
                    label10.Text = "";
                    label11.Text = "";
                    label6.Text = "";
                    pictureBox2.Image = null;
                }
                dr1.Close();
                con1.Close();
                con1.Dispose();
                dr1.Dispose();

                ////////////////////EQUIPO LOGO
                SqlConnection con2 = new SqlConnection(connectionString);
                string sql2 = "SELECT LOGO";

                sql2 += " FROM EQUIPOS WHERE ";
                sql2 += " nombrecorto='" + dr[1].ToString().Trim() + "'";
                

                SqlCommand obj2 = new SqlCommand(sql2, con2);
                obj2.CommandType = CommandType.Text;
                con2.Open();

                SqlDataReader dr2 = obj2.ExecuteReader();
                if (dr2.HasRows)
                {
                    dr2.Read();
                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(dr2[0]);
                        MemoryStream mem = new MemoryStream(data);
                        pictureBox1.Image = Image.FromStream(mem);
                    }
                    catch { pictureBox1.Image = null; }
                }
                else
                {
                    pictureBox1.Image = null;
                }
                dr2.Close();
                con2.Close();
                con2.Dispose();
                dr2.Dispose();
                ////////////////////goles partido
                SqlConnection con3 = new SqlConnection(connectionString);
                string sql3 = "SELECT count(*)";

                sql3 += " FROM SCORE WHERE NUMERO= " + dr[0].ToString().Trim();
                sql3 += " AND EQUIPO='" + dr[1].ToString().Trim() + "'";


                SqlCommand obj3 = new SqlCommand(sql3, con3);
                obj3.CommandType = CommandType.Text;
                con3.Open();

                SqlDataReader dr3 = obj3.ExecuteReader();
                if (dr3.HasRows)
                {
                    dr3.Read();
                    label7.Text = dr3[0].ToString().Trim();
                }
                else
                {
                    label7.Text = "";
                }
                dr3.Close();
                con3.Close();
                con3.Dispose();
                dr3.Dispose();
            }
            else
            {
                label10.Text = "";
                label11.Text = "";
                pictureBox2.Image = null;
                pictureBox1.Image = null;
                label7.Text = "";
                label6.Text = "";
            }
            dr.Close();
            con.Close();
            con.Dispose();
            dr.Dispose();
        }

        private void Campeonato()
        {
            label12.Text = DateTime.Now.Year.ToString("0000");


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
                    label12.Text = dr[0].ToString().Trim();

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
    }
}
