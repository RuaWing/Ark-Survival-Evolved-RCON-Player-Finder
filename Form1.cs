using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Drawing.Text;
using System.Timers;


namespace Ark_Survival_Evolved_RCON_Player_Finder
{
    public partial class Form1 : Form
    {
        // Create an instance of RconClient pointing to an IP and a PORT

        private readonly List<Server_Info> servers = new();


        public Form1()
        {
            InitializeComponent();

            // load the config.json file
            string json_file_as_string = File.ReadAllText("config.json");

            // load the list of friendly names from the 
            JArray? friendly_names = JObject.Parse(json_file_as_string)["friendly names"] as JArray;

            List<String> friendly_names_list = new();
            // iterate over each friendly name found and add it to the friendly names list.
            foreach (JObject item in friendly_names!)
            {
                string? name = item.GetValue("name")?.ToString();
                friendly_names_list.Add(name);
            }

            // load the list of servers from the config.json file.
            JArray? server_info = JObject.Parse(json_file_as_string)["servers"] as JArray;

            // iterate over each server and create a new Server_Info instance with the data scraped from the config.json file.
            foreach (JObject item in server_info!)
            {
                string? serverName = item.GetValue("serverName")?.ToString();
                string? mapName = item.GetValue("mapName")?.ToString();
                string? ip = item.GetValue("ip")?.ToString();
                int? port = (int?)item.GetValue("port");
                string? password = item.GetValue("password")?.ToString();

                // add new server to the server list;
                Server_Info server = new()
                {
                    serverName = serverName,
                    mapName = mapName,
                    ip = ip,
                    port = port,
                    password = password,
                    friendly_names = friendly_names_list
                };
                this.servers.Add(server);

                // add each tab
                TabPage tabPage = new()
                {
                    Text = $"{serverName}"
                };
                tabControl1.Controls.Add(tabPage);

                // add server info
                Label label_server_name = new();
                Label label_map_name = new();
                Label label_server_port = new();
                Label label_server_ip = new();
                Label label_operation = new();
                Label label_interaval = new();

                label_server_name.AutoSize = true;
                label_server_name.Location = new System.Drawing.Point(10, 22);
                label_server_name.Name = "Server Name";
                label_server_name.Size = new System.Drawing.Size(50, 20);
                label_server_name.TabIndex = 7;
                label_server_name.Text = "Server Name: " + server.serverName;

                label_map_name.AutoSize = true;
                label_map_name.Location = new System.Drawing.Point(10, 42);
                label_map_name.Name = "Map Name";
                label_map_name.Size = new System.Drawing.Size(50, 20);
                label_map_name.TabIndex = 8;
                label_map_name.Text = "Map Name: " + server.mapName;

                label_server_port.AutoSize = true;
                label_server_port.Location = new System.Drawing.Point(10, 62);
                label_server_port.Name = "Server Port";
                label_server_port.Size = new System.Drawing.Size(50, 20);
                label_server_port.TabIndex = 9;
                label_server_port.Text = "Server Port: " + server.port;

                label_server_ip.AutoSize = true;
                label_server_ip.Location = new System.Drawing.Point(10, 82);
                label_server_ip.Name = "Server IP: ";
                label_server_ip.Size = new System.Drawing.Size(50, 20);
                label_server_ip.TabIndex = 10;
                label_server_ip.Text = "Server IP: " + server.ip;

                tabPage.Controls.Add(label_server_name);
                tabPage.Controls.Add(label_map_name);
                tabPage.Controls.Add(label_server_port);
                tabPage.Controls.Add(label_server_ip);

                // add player list text box

                RichTextBox richTextBox_player_list = new()
                {
                    Location = new System.Drawing.Point(10, 150),
                    Name = "richTextBox_" + server.serverName + server.mapName,
                    Size = new System.Drawing.Size(300, 400),
                    TabIndex = 6,
                    Text = ""
                };
                tabPage.Controls.Add(richTextBox_player_list);

                // add player list label

                Label label_player_list = new()
                {
                    AutoSize = true,
                    Location = new System.Drawing.Point(10, 220),
                    Size = new System.Drawing.Size(148, 20),
                    TabIndex = 7,
                    Text = "Player List"
                };
                tabPage.Controls.Add(label_player_list);

                // add picture box
                PictureBox pictureBox_map = new()
                {
                    Name = "picturebox_" + mapName,
                    Location = new System.Drawing.Point(350, 10),
                    Size = new System.Drawing.Size(600, 600),
                    TabIndex = 6,
                    TabStop = false
                };

                server.tab = tabPage;

                string map_image_file_name = "media/" + mapName + ".jpg";

                pictureBox_map.Image = Image.FromFile(map_image_file_name);
                pictureBox_map.SizeMode = PictureBoxSizeMode.StretchImage;
                tabPage.Controls.Add(pictureBox_map);

                // get the map pictures size and coordinates for the server info
                server.map_root_height = pictureBox_map.Location.X;
                server.map_root_width = pictureBox_map.Location.Y;
                server.map_size_height = pictureBox_map.Size.Width;
                server.map_size_width = pictureBox_map.Size.Height;
            }
        }

        private void Button_connect_Click(object sender, EventArgs e)
        {
            Boolean start = false;

            if (this.button_connect.Text == "START") {
                this.button_connect.Text = "STOP";
                start = true;

                labelOperation.ForeColor = Color.Green;
                labelOperation.Text = "Running...";
            }
            else
            {
                this.button_connect.Text = "START";
                start = false;

                labelOperation.ForeColor = Color.Red;
                labelOperation.Text = "Stopped.";
            }

            foreach (Server_Info server in servers)
            {
                //find the player list textbox to write back to
                // asign it to the server
                server.RichTextBox_local_list_textbox = (RichTextBox?)tabControl1.Controls.Find("richTextBox_" + server.serverName + server.mapName, true).FirstOrDefault();
                server.send_command_timer.Interval = (double)this.intervalNumericUpDown1.Value;
                server.send_command_timer.Enabled = start;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            foreach (Server_Info server in servers)
            {
                server.send_command_timer.Interval = (double)this.intervalNumericUpDown1.Value;
            }
        }
    }
}