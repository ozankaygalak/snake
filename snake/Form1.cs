using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace snake
{
    public partial class Form1 : Form
    {
        private List<Point> snake;
        private Point food;
        private int direction; // 0: sağ, 1: sol, 2: yukarı, 3: aşağı
        private Timer gameTimer;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            snake = new List<Point>();
            snake.Add(new Point(0, 0)); // Sol üst köşede başlat
            direction = 0; // Başlangıçta sağa doğru hareket etsin

            GenerateFood(); // İlk yemi oluştur

            gameTimer = new Timer();
            gameTimer.Interval = 100; // Timer interval (milisaniye cinsinden)
            gameTimer.Tick += GameTick;
            gameTimer.Start();
        }

        private void GenerateFood()
        {
            Random rand = new Random();
            food = new Point(rand.Next(0, pictureBox1.Width / 10) * 10, rand.Next(0, pictureBox1.Height / 10) * 10);
        }

        private void GameTick(object sender, EventArgs e)
        {
            MoveSnake();
            CheckCollision();
            CheckFood();
            pictureBox1.Invalidate(); // PictureBox'u yeniden çiz
        }

        private void MoveSnake()
        {
            Point newHead = snake[0];

            switch (direction)
            {
                case 0:
                    newHead.X += 10;
                    break;
                case 1:
                    newHead.X -= 10;
                    break;
                case 2:
                    newHead.Y -= 10;
                    break;
                case 3:
                    newHead.Y += 10;
                    break;
            }

            snake.Insert(0, newHead);

            if (snake.Count > 1)
                snake.RemoveAt(snake.Count - 1);
        }

        private void CheckCollision()
        {
            // Duvar çarpışması
            if (snake[0].X < 0 || snake[0].X >= pictureBox1.Width || snake[0].Y < 0 || snake[0].Y >= pictureBox1.Height)
                EndGame();

            // Kendi kendine çarpma
            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[0] == snake[i])
                    EndGame();
            }
        }

        private void CheckFood()
        {
            // Yem yendiğinde
            if (snake[0] == food)
            {
                snake.Add(new Point(-10, -10)); // Yılanın kuyruğuna yeni bir parça ekle
                GenerateFood(); // Yeni yem oluştur
            }
        }

        private void EndGame()
        {
            gameTimer.Stop();
            MessageBox.Show("Oyun bitti!");
            InitializeGame();
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // PictureBox üzerine yılanı ve yemi çiz
            Graphics g = e.Graphics;

            // Yılanı çiz
            foreach (Point segment in snake)
            {
                g.FillRectangle(Brushes.Green, segment.X, segment.Y, 10, 10);
            }

            // Yemi çiz
            g.FillEllipse(Brushes.Red, food.X, food.Y, 10, 10);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Yön tuşlarına basıldığında yılanın hareket yönünü güncelle
            switch (e.KeyCode)
            {
                case Keys.Right:
                    if (direction != 1) // Sağa gidiliyorsa sola gitmesini engelle
                        direction = 0;
                    break;
                case Keys.Left:
                    if (direction != 0) // Sola gidiliyorsa sağa gitmesini engelle
                        direction = 1;
                    break;
                case Keys.Up:
                    if (direction != 3) // Yukarı gidiliyorsa aşağı gitmesini engelle
                        direction = 2;
                    break;
                case Keys.Down:
                    if (direction != 2) // Aşağı gidiliyorsa yukarı gitmesini engelle
                        direction = 3;
                    break;
            }
        }
    }
}
