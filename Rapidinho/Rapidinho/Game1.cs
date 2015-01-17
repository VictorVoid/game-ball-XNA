using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Rapidinho
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //atributos
       
        Texture2D jogador1;
        int jogDX = 5, jogDY = 5;
       
        Vector2 jogador1Pos;
      
        Rectangle jogador1Rec;
        Vector2 initilPosition1;
        Vector2 initilPosition2;
        Rectangle line;
        Vector2 linePos = new Vector2(200, 0);
        Texture2D texturaPonto1, texturaPonto2;
        Rectangle recPonto1, recPonto2;
        int pontJogador1 = 0, pontJogador2 = 0;
        SpriteFont fonte;
        bool campeao = false;
        bool campeaoEstrela = false;
        string nameCampeao = "";
        bool enter = false;
        int fase = 1;
        //  Boolean tocou = false; // para saber se tocou em outra bola, se tocar não vai pegar a seta até a bola voltar
        

        #region inimigo


        //Vector2 inimigoPos = new Vector2 (400,0);
        Vector2 inimigoPos = new Vector2(300, 0), inimigoPos2 = new Vector2(350, 0), inimigoPos3 = new Vector2(0, 100), inimigoPos4 = new Vector2(500,200), inimigoPos5 = new Vector2 (0, 300);
        int[] dx = new int[6], dy = new int[6];
        int velPositivo = 10, velNegativo = -10;
        Rectangle inimigoRec, inimigo2Rec, inimigo3Rec, inimigo4Rec, inimigo5Rec;
        List<Texture2D> inimigo = new List<Texture2D>();
        

        #endregion
             #region coracao
        List<Texture2D> coracao = new List<Texture2D>();
        Vector2 coracao1pos = new Vector2(350,100);
        Texture2D textCoracao;
        Rectangle recCoracao1;
        bool [] pontoCoracao = new bool [4]; 

#endregion
        
    //campeao
        Texture2D ganhou;
        #region obstaculos
        Vector2 obstaculopos = new Vector2(320, 150);
        Texture2D texturaObstaculo;
        Rectangle recObstaculo;
        bool obstaculoA = false;
        bool obstaculoB = false;
        bool obstaculoC = false;
        Vector2 obstaculo2pos = new Vector2(430, 170);
        Rectangle recObstaculo2;
        Vector2 obstaculo3pos = new Vector2(700, 170);
        Rectangle recObstaculo3;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            Window.Title = "SEJE RÁPIDO";
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            base.Initialize();

        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
        
            jogador1 = Content.Load<Texture2D>("soccer-ball-li");


            texturaPonto1 = Content.Load<Texture2D>("ponto1");
            texturaPonto2 = Content.Load<Texture2D>("ponto2");

            
            jogador1Rec = new Rectangle((int)jogador1Pos.X, (int)jogador1Pos.Y, jogador1.Width, jogador1.Height);

            recPonto1 = new Rectangle(0, 200, texturaPonto1.Width, texturaPonto1.Height);
            recPonto2 = new Rectangle(Window.ClientBounds.Width - texturaPonto2.Width, 200, texturaPonto2.Width, texturaPonto2.Height);

            initilPosition1 = new Vector2(texturaPonto1.Width, 225);
            
            jogador1Pos = initilPosition1;
            
            //obstaculos
            texturaObstaculo = Content.Load<Texture2D>("obstaculo");
            recObstaculo = new Rectangle((int)obstaculopos.X,(int)obstaculopos.Y,texturaObstaculo.Width, texturaObstaculo.Height);

            #region INIMIGO
            inimigo.Insert(0, jogador1); // list.Insert (indice do list que ta usando, Texture2D que vai usar);
            inimigo.Insert(1, jogador1);
            inimigo.Insert(2, jogador1);
            inimigo.Insert(3, jogador1);
            inimigo.Insert(4, jogador1);
            //retângulo para colisão
            inimigoRec = new Rectangle((int)inimigoPos.X, (int)inimigoPos.Y, inimigo[0].Width, inimigo[0].Height);
            inimigo2Rec = new Rectangle((int)inimigoPos2.X, (int)inimigoPos2.Y, inimigo[1].Width, inimigo[1].Height);
            inimigo3Rec = new Rectangle((int)inimigoPos3.X, (int)inimigoPos3.Y, inimigo[2].Width, inimigo[2].Height);
            //Coordenadas do inimigo
            for (int i = 0; i < 5; i++ ){
                dx[i] = 10;
                dy[i] = 10;
            }
            #endregion
                //fonte
                fonte = Content.Load<SpriteFont>("fonte");

            #region estrelas
            textCoracao = Content.Load<Texture2D>("coracao-emoticon");
            coracao.Insert(0, textCoracao);
            coracao.Insert(1, textCoracao);
            coracao.Insert(2, textCoracao);
            coracao.Insert(3, textCoracao);
            recCoracao1 = new Rectangle((int)coracao1pos.X, (int)coracao1pos.Y, textCoracao.Width, textCoracao.Height );
            
            //obstaculos:
            recObstaculo2 = new Rectangle((int)obstaculo2pos.X, (int)obstaculo2pos.Y, texturaObstaculo.Width, texturaObstaculo.Height);
            recObstaculo3 = new Rectangle((int)obstaculo3pos.X, (int)obstaculo3pos.Y, texturaObstaculo.Width, texturaObstaculo.Height);

            //adicionando tudo falso
            reiniciarCoracao();
            #endregion
            //campeao
            ganhou = Content.Load<Texture2D>("ganhou");

            

        }
        public void reiniciarCoracao()
        {
            for (int i = 0; i < 4; i++)
                pontoCoracao[i] = false; 
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)//tira uma foto do teclado, e pergunta 
                this.Exit();
            jogador1Rec.X = (int)jogador1Pos.X; //atualizando os valores do retangulo
            jogador1Rec.Y = (int)jogador1Pos.Y;

         //atualizando rec CORAÇÃO
            recCoracao1 = new Rectangle((int)coracao1pos.X, (int)coracao1pos.Y, textCoracao.Width, textCoracao.Height);

            #region INIMIGO
            //atualizando o retângulo para colisão
            inimigoRec = new Rectangle((int)inimigoPos.X, (int)inimigoPos.Y, inimigo[0].Width, inimigo[0].Height);
            inimigo2Rec = new Rectangle((int)inimigoPos2.X, (int)inimigoPos2.Y, inimigo[1].Width, inimigo[1].Height);
            inimigo3Rec = new Rectangle((int)inimigoPos3.X, (int)inimigoPos3.Y, inimigo[1].Width, inimigo[1].Height);
            
            //incrementando o loop
            if (fase == 1)
            inimigoPos.Y += dy[0];

            if (fase == 2)
            {
                inimigoPos.Y += dy[0];
                if (obstaculoA)
                {
                    inimigoPos2.Y += dy[1];
                }
            }
            
            /**************INIMIGO 1*****************/
            if (inimigoPos.X > Window.ClientBounds.Width - inimigo[0].Width)
            {
                dx[0] = velNegativo;
            }
            if (inimigoPos.X < 0)
            {
                dx[0] = velPositivo;
            }
            if (inimigoPos.Y > Window.ClientBounds.Height - inimigo[0].Height)
            {
                dy[0] = velNegativo;
            }
            if (inimigoPos.Y < 0)
            {
                dy[0] = velPositivo;
            }
            /**************INIMIGO 2 *************/
            if (inimigoPos2.X > Window.ClientBounds.Width - inimigo[0].Width)
            {
                dx[1] = velNegativo;
            }
            if (inimigoPos2.X < 0 || inimigoPos2.X < 0)
            {
                dx[1] = velPositivo;
            }
            if (inimigoPos2.Y > Window.ClientBounds.Height - inimigo[0].Height)
            {
                dy[1] = velNegativo;
            }
            if (inimigoPos2.Y < 0)
            {
                dy[1] = velPositivo;
            }
            /*****************INIMIGO 3*******************/

            if (inimigoPos3.X > Window.ClientBounds.Width - inimigo[0].Width)
            {
                dx[2] = velNegativo;
            }
            if (inimigoPos3.X < 0)
            {
                dx[2] = velPositivo;
            }
            if (inimigoPos3.Y > Window.ClientBounds.Height - inimigo[0].Height)
            {
                dy[2] = velNegativo;
            }
            if (inimigoPos3.Y < 0)
            {
                dy[2] = velPositivo;
            }
          
                /********************INIMIGO 4*************************/
                if (inimigoPos4.X > Window.ClientBounds.Width - inimigo[0].Width)
                {
                    dx[3] = velNegativo;
                }
                if (inimigoPos4.X < 0)
                {
                    dx[3] = velPositivo;
                }
                if (inimigoPos4.Y > Window.ClientBounds.Height - inimigo[0].Height)
                {
                    dy[3] = velNegativo;
                }
                if (inimigoPos4.Y < 0)
                {
                    dy[3] = velPositivo;
                }

                /********************INIMIGO 5*************************/
                if (inimigoPos5.X > Window.ClientBounds.Width - inimigo[0].Width)
                {
                    dx[4] = velNegativo;
                }
                if (inimigoPos5.X < 0)
                {
                    dx[4] = velPositivo;
                }
                if (inimigoPos5.Y > Window.ClientBounds.Height - inimigo[0].Height)
                {
                    dy[4] = velNegativo;
                }
                if (inimigoPos5.Y < 0)
                {
                    dy[4] = velPositivo;
                }
            
             
            

            #region colisaoInimigo
            if (inimigoRec.Intersects(jogador1Rec) || inimigo2Rec.Intersects(jogador1Rec) || inimigo3Rec.Intersects(jogador1Rec) || inimigo4Rec.Intersects(jogador1Rec) || inimigo5Rec.Intersects(jogador1Rec) )
            { if (!campeao)
                    jogador1Pos = initilPosition1; 
                    reiniciarCoracao();
                    reiniciaObstaculos();
            }
            
            
            #endregion
            #endregion

            #region colisão

          

            // colisão no quadrado de pontos;
            if (jogador1Rec.Intersects(recPonto2))
            {
               if (fase == 1)
                if (pontoCoracao[0] == true ) // verificando se pegou todos os corãções
                {
                    campeao = true;

                }
                if (fase == 2)
                    if (pontoCoracao[0] == true)
                    {
                        campeao = true;
                    }

            }
          
            #endregion

            #region Controle1
            if (enter == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    campeao = false; reiniciarCoracao();
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                jogador1Pos.X -= jogDX;
                if (jogador1Pos.X < 0)
                {
                    jogador1Pos.X += jogDX;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                jogador1Pos.X += jogDX;
                if (jogador1Pos.X > Window.ClientBounds.Width - jogador1.Width)
                {
                    jogador1Pos.X -= jogDX;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                jogador1Pos.Y += jogDY;
                if (jogador1Pos.Y > Window.ClientBounds.Height - jogador1.Height)
                {
                    jogador1Pos.Y -= jogDY;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                jogador1Pos.Y -= jogDY;
                if (jogador1Pos.Y < 0)
                {
                    jogador1Pos.Y += jogDY;
                }
            }
            #endregion




            #region Coracao
            #region CoracaoColisao
            //coracao 1:
                    if (recCoracao1.Intersects(jogador1Rec))
                {
                    pontoCoracao[0] = true;
                }
            #endregion
           if (fase == 2){
               coracao1pos.X = 340;
               coracao1pos.Y = 200;
                
           }
            


            #endregion

           #region obstaculo
            //colisoes

        
           if (fase == 2)
           {
               if (jogador1Rec.Intersects(recObstaculo))
               {
                   jogador1Pos = initilPosition1;
                   reiniciarCoracao();
                   reiniciaObstaculos();
                   
               }
               if (obstaculoB) // tem que ta ativado para poder executar
               {
                   if (jogador1Rec.Intersects(recObstaculo2))
                   {
                       jogador1Pos = initilPosition1;
                       reiniciarCoracao();
                       reiniciaObstaculos();

                   }
               }
               if (obstaculoC)
               {
                   if (jogador1Rec.Intersects(recObstaculo3))
                   {
                       jogador1Pos = initilPosition1;
                       reiniciarCoracao();
                       reiniciaObstaculos();
                   }

               }

               if (jogador1Pos.X > obstaculopos.X)
               {
                   obstaculoA = true;
               }
               if (pontoCoracao[0] == true && jogador1Pos.X > 380 && jogador1Pos.Y > 180)
               {
                   obstaculoB = true;
               }
               if (pontoCoracao[0] == true && jogador1Pos.X > 660 && jogador1Pos.Y > 180)
               {
                   obstaculoC = true;
               }

           }


           #endregion


           if (enter == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    campeao = false;
                    reiniciarCoracao();
                    jogador1Pos = initilPosition1;
                    
                    fase+=1;

                }
                enter = false;
               
            }

            base.Update(gameTime);
        }
        //MÉTODO PARA REINICIAR TODOS OS OBSTÁCULOS
        void reiniciaObstaculos() {
            inimigoPos2.Y = 0;  // para o obstaculoA(bolinha)ficar no 0, lá em cima
            obstaculoC = false;
            obstaculoB = false;
            obstaculoA = false; // para a bolinha sumir e voltar do início
        }

        protected override void Draw(GameTime gameTime)
        {
           
            spriteBatch.Begin();
            if (!campeao)
            {   GraphicsDevice.Clear(Color.CornflowerBlue);

                //desenhando quadrado de pontos
                spriteBatch.Draw(texturaPonto1, new Vector2(0, 200), Color.White);
                spriteBatch.Draw(texturaPonto2, new Vector2(Window.ClientBounds.Width - texturaPonto2.Width, 200), Color.White);
                // desenhando os jogadores
               // spriteBatch.Draw(jogador2, jogador2Pos, Color.Yellow); // jogador 2

                spriteBatch.Draw(jogador1, jogador1Pos, Color.Red); // jogador1
           //     spriteBatch.DrawString(fonte, pontJogador1.ToString(), new Vector2(0, 0), Color.Black);
             //   spriteBatch.DrawString(fonte, pontJogador2.ToString(), new Vector2(788, 0), Color.Black);
                
            }
            else
            {
             //  spriteBatch.Draw(ganhou, new Vector2(160, 37), Color.White);
               GraphicsDevice.Clear(Color.White);
               spriteBatch.DrawString(fonte, "Pressione Enter", new Vector2(600, 400), Color.Red);
              
               enter = true;
            }
            //inimigo
            
            if (fase == 1 && enter == false)
            {
                spriteBatch.Draw(inimigo[0], inimigoPos, Color.Black);
                if (pontoCoracao[0] == true)
                {
                    
                    spriteBatch.DrawString(fonte, "Chegue ate o quadrado amarelo", new Vector2(200, 400), Color.Yellow);
                    
                }
            }
           if (fase == 2 && enter == false)
           {
               spriteBatch.Draw(inimigo[0], inimigoPos, Color.Blue);
               spriteBatch.Draw(texturaObstaculo, obstaculopos, Color.White);
               if (obstaculoA)
               {
                   spriteBatch.Draw(inimigo[1], inimigoPos2, Color.Yellow);
               }
               if (obstaculoB)
               {
                   spriteBatch.Draw(texturaObstaculo,obstaculo2pos, Color.White);
               }
               if (obstaculoC)
               {
                   spriteBatch.Draw(texturaObstaculo, obstaculo3pos, Color.Red);
               }
           }
            
         
            
         /****ESTRELAS****/


            if (pontoCoracao[0] == false) spriteBatch.Draw(coracao[0], new Vector2((int)coracao1pos.X,(int)coracao1pos.Y), Color.White);

            spriteBatch.End();
            


            base.Draw(gameTime);
        }
    }
}
