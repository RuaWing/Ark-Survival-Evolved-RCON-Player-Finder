﻿using RconSharp;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace Ark_Survival_Evolved_RCON_Player_Finder
{
    public class Server_Info
    {
        /// <summary>
        /// Name that is taken from the config.json file. 
        /// Is used to set the title of each map.
        /// </summary>
        public string? mapName = "";

        /// <summary>
        /// Name that is taken from the config.json file. 
        /// Is used to set the title of each server.
        /// </summary>
        public string? serverName = "";

        /// <summary>
        /// IP address that is taken from the config.json file. 
        /// Used to connect to the RCON.
        /// </summary>
        public string? ip = "";

        /// <summary>
        /// The port that is taken from the config.json file. 
        /// Used to create the connection the RCON server.
        /// </summary>
        public int? port = 0;

        /// <summary>
        /// The RCON/Admin password taken from the config.json file.
        /// Used to authenticate the connection between the server and the application.
        /// </summary>
        public string? password = "";


        // the rcon client that is used to create a connection to the server.
        private RconClient? client;

        /// <summary>
        /// The textbox that is used to display the data that arives from each server.
        /// </summary>
        public RichTextBox? RichTextBox_local_list_textbox;

        /// <summary>
        /// The timer that is used to trigger the request for an update of data from the server. 
        /// </summary>
        public System.Timers.Timer send_command_timer = new();

        // List of players that have been detected from the response from the server.
        private List<Player_info> player_list = new();

        /// <summary>
        /// Tab page that each server will use in the application.
        /// </summary>
        public TabPage? tab;

        /// <summary>
        /// The width position of the map image.
        /// </summary>
        public int map_root_width;

        /// <summary>
        /// The height position of the map image.
        /// </summary>
        public int map_root_height;

        /// <summary>
        /// The size of the width of the map image.
        /// </summary>
        public int map_size_width;

        /// <summary>
        /// The size of the height of the map image.
        /// </summary>
        public int map_size_height;

        /// <summary>
        /// A list of friendly names that will be highlighted green by the UI.
        /// </summary>
        public List<string> friendly_names = new();

        public bool in_progress = false;


        /// <summary>
        /// Constructor for the Server_Info class.
        /// Configures the timer to apply the send command function when a new instance of Server_Info is created.
        /// </summary>
        public Server_Info()
        {
            // Tell the timer what to do when it elapses
            send_command_timer.Elapsed += (sender, e) => Send_command_to_Server(sender!, e);
            // Set it to go off every 2 seconds and repeat.
            send_command_timer.Interval = 2000;
            send_command_timer.Enabled = false;
            send_command_timer.AutoReset = true;
        }

        // Attempts to send the command "listallplayerpos" to the RCON. 
        // Creates a connection if the connection does not yet exsist of has died.
        // Once the data has been retured, this function then displays the data using the "Parse_response_string" function.
        private async void Send_command_to_Server(object sender, ElapsedEventArgs e)
        {
            // only attempt to send data when no other attempt is in progress.
            if (this.in_progress == false)
            {
                this.in_progress = true;
                string response = "";

                // Create the connection if not already authed.

                try
                {
                    this.client = RconClient.Create(this.ip, (ushort)port!);

                    // TRY LOCKING THIS METHOD?

                    await client.ConnectAsync();

                    var authenticated = await client.AuthenticateAsync(password);

                    if (authenticated)
                    {
                        // Send the command to the server
                        response = await client.ExecuteCommandAsync("listallplayerpos");

                        Parse_response_string(response);

                        client.Disconnect();
                        client = null;
                    }
                    else
                    {
                        client.Disconnect();
                        client = null;

                        this.RichTextBox_local_list_textbox!.BeginInvoke((MethodInvoker)delegate () { RichTextBox_local_list_textbox.Text = "No Response"; });
                    }


                }
                catch (Exception ex)
                {
                    if (client != null)
                    {
                        client.Disconnect();
                    }
                    client = null;

                    this.RichTextBox_local_list_textbox!.BeginInvoke((MethodInvoker)delegate () { RichTextBox_local_list_textbox.Text = ex.ToString(); });
                }
                this.in_progress = false;
            }
        }


        // Creates the UI dots and lables for each player.
        // Forumlas used to convert UE4 coordinates to ark lat / lon coordinates can be found at https://ark.fandom.com/wiki/Coordinates as of 01/09/2022
        // New source of information on coordinate transformations https://ark.wiki.gg/wiki/Coordinates as of 04/12/2023
        // This function will attempt to convert coordinates for each map.
        private void Create_player_dots(Player_info player)
        {

            this.tab!.BeginInvoke((MethodInvoker)delegate ()
            {
                // Convert ue4 coordinate to ark lat/lon coordinates for each map.

                float shift_x;
                float shift_y;

                float mult_x;
                float mult_y;

                float lat = 0;
                float lon = 0;

                if (this.mapName == "the center")
                {
                    shift_x = 55.1f;
                    shift_y = 30.34f;

                    mult_x = 9600f;
                    mult_y = 9584f;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "aberration")
                {
                    shift_x = 50;
                    shift_y = 50;

                    mult_x = 8000;
                    mult_y = 8000;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "crystal")
                {
                    shift_x = 48.75f;
                    shift_y = 50f;

                    mult_x = 16000f;
                    mult_y = 70000f;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "extinction")
                {
                    shift_x = 50f;
                    shift_y = 50f;

                    mult_x = 8000f;
                    mult_y = 8000f;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "fjordur")
                {
                    shift_x = 50f;
                    shift_y = 50f;

                    mult_x = 7141f;
                    mult_y = 7141f;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "genesis 1")
                {
                    shift_x = 50f;
                    shift_y = 50f;

                    mult_x = 10500f;
                    mult_y = 10500f;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "genesis 2")
                {
                    shift_x = 49.655f;
                    shift_y = 49.655f;

                    mult_x = 14500f;
                    mult_y = 14500f;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "the island")
                {
                    shift_x = 50f;
                    shift_y = 50f;

                    mult_x = 8000f;
                    mult_y = 8000f;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "the island")
                {
                    shift_x = 50f;
                    shift_y = 50f;

                    mult_x = 8000f;
                    mult_y = 8000f;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "lost island")
                {
                    shift_x = 51.634f;
                    shift_y = 49.02f;

                    mult_x = 15300;
                    mult_y = 15300;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "ragnarok")
                {
                    shift_x = 50;
                    shift_y = 50;

                    mult_x = 13100;
                    mult_y = 13100;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "scorched earth")
                {
                    shift_x = 50;
                    shift_y = 50;

                    mult_x = 8000;
                    mult_y = 8000;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }
                else if (this.mapName == "valguero")
                {
                    shift_x = 50;
                    shift_y = 50;

                    mult_x = 8160;
                    mult_y = 8160;

                    lat = shift_y + (player.y / mult_y);
                    lon = shift_x + (player.x / mult_x);
                }

                // Add the player name and information to the player list text box.
                RichTextBox_local_list_textbox!.AppendText(player.name + " lat: " + lat.ToString("F2") + " , lon: " + lon.ToString("F2") + "\n");

                // Calculate screen offsets for the map UI image. (May be an imperfect fit, adjust to your use case.)
                float red_dot_pos_width = 350 + (lon * 6);
                float red_dot_pos_height = 10 + (lat * 6);


                // before drawing the player dots and names UI features, first identify if the player is a friendly player.

                bool is_friendly = false;
                foreach (string friendly_name in this.friendly_names)
                {
                    if (player.name.Contains(friendly_name))
                    {
                        is_friendly = true;
                    }
                }

                // create the player dot and add it to the tab page.

                if (!is_friendly)
                {
                    // draw a red dot as the user is not on the list of friendly names.

                    PictureBox pictureBox_dot = new()
                    {
                        Name = "dot",
                        Location = new System.Drawing.Point((int)red_dot_pos_width, (int)red_dot_pos_height),
                        Size = new System.Drawing.Size(8, 8),
                        TabStop = false,
                        Image = Image.FromFile("media/red_dot.png"),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        BackColor = Color.Transparent
                    };

                    tab.Controls.Add(pictureBox_dot);
                    pictureBox_dot.BringToFront();
                    player.icon = pictureBox_dot;

                    // add the player label to the tab page.
                    Label label_player_name = new()
                    {
                        Name = "dot", // named dot so it gets deleted
                        AutoSize = true,
                        Location = new System.Drawing.Point((int)red_dot_pos_width + 15, (int)red_dot_pos_height + 5),
                        Size = new System.Drawing.Size(100, 20),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Text = player.simpleName
                    };

                    tab.Controls.Add(label_player_name);
                    label_player_name.BringToFront();

                }
                else
                {
                    // the player is on the list of friendly names, draw a green dot.

                    PictureBox pictureBox_dot = new()
                    {
                        Name = "dot",
                        Location = new System.Drawing.Point((int)red_dot_pos_width, (int)red_dot_pos_height),
                        Size = new System.Drawing.Size(8, 8),
                        TabStop = false,
                        Image = Image.FromFile("media/green_dot.png"),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        BackColor = Color.Transparent
                    };

                    tab.Controls.Add(pictureBox_dot);
                    pictureBox_dot.BringToFront();
                    player.icon = pictureBox_dot;

                    // add the player label to the tab page.
                    Label label_player_name = new()
                    {
                        Name = "dot", // named dot so it gets deleted
                        AutoSize = true,
                        Location = new System.Drawing.Point((int)red_dot_pos_width + 15, (int)red_dot_pos_height + 5),
                        Size = new System.Drawing.Size(100, 20),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Text = player.simpleName
                    };

                    tab.Controls.Add(label_player_name);
                    label_player_name.BringToFront();


                }

            });
        }


        // finds all UI elements named "dot" and deletes them.
        // used to clear the UI before the creation of the new up to date player information.
        private void Delete_player_dots()
        {
            this.tab!.BeginInvoke((MethodInvoker)delegate ()
            {
                Control[] boxs = tab.Controls.Find("dot", true);

                foreach (Control box in boxs)
                {
                    box.Dispose();
                }
            });

        }


        // Used to find all players and coordinates that are returned from the "listallplayerpos" response string.
        // players are then added to the player_list.
        private void Parse_response_string(string response)
        {
            // clear the local list textbox
            this.RichTextBox_local_list_textbox!.BeginInvoke((MethodInvoker)delegate () { RichTextBox_local_list_textbox.Text = "\n"; });

            // delete all old player dots
            Delete_player_dots();

            // clear the list of players
            player_list = new List<Player_info>();

            //split the response string into individual lines
            string[] indivdual_lines = response.Split("\n");

            // iterate over these lines
            foreach (string line in indivdual_lines)
            {
                // if a line contains the beginning of a coordinate
                if (line.Contains("), X="))
                {
                    // get the vector
                    string vector = line.Split("), X=")[1];
                    string name = line.Split(", X=")[0];

                    // create a new player object to add the players info to.
                    Player_info player = new()
                    {
                        simpleName = name.Split(" - ")[0],
                        name = name,
                        x = float.Parse(vector.Split("Y=")[0]),
                        y = float.Parse(vector.Split("Y=")[1].Split("Z=")[0]),
                        z = float.Parse(vector.Split("Z=")[1])
                    };

                    // add the player to the player list
                    player_list.Add(player);

                    // create new player dots
                    Create_player_dots(player);
                }
            }
        }
    }


    // player_info struct that contains information on the player
    public struct Player_info
    {
        public string simpleName = "";
        public string name = "";

        public float x = 0;
        public float y = 0;
        public float z = 0;

        public PictureBox? icon = null;

        public Player_info()
        {
        }
    }
}




