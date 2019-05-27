using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Activation;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using SpaceTaxi_1.Customer;
using SpaceTaxi_1.GameStates;

namespace SpaceTaxi_1 {
    public class Collision {
        private List<Image> explosionStrides;
        public AnimationContainer explosions;
        public Text GameOver;
        private int explosionLength;


        public Collision() {
            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(500);
            explosionLength = 500;

            GameOver = new Text("Game's Over", new Vec2F(0.42f, 0.5f),
                new Vec2F(0.3f, 0.3f));
            GameOver.SetText("Game's Over\nPress Space");
            GameOver.SetColor(new Vec3I(0, 255, 0));
        }
        
        /// <summary>
        /// Adds explosion upon collision
        /// </summary>
        /// <param name="posX"></param>
        /// Position x of explosion
        /// <param name="posY"></param>
        /// Position y of explosion
        /// <param name="extentX">
        /// Extend x of explosion
        /// </param>
        /// <param name="extentY">
        ///Extend y of explosion
        /// </param>

        private void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation(
                new StationaryShape(posX, posY, extentX, extentY), explosionLength,
                new ImageStride(explosionLength / 8, explosionStrides));
        }

        /// <summary>
        /// Handles the collisions and the level changing
        /// </summary>
        /// <param name="Entities">
        /// List of entities
        /// </param>
        /// <param name="player">
        /// Player
        /// </param>

        public void Collisions(EntityContainer<Entity> Entities, Player player) {
            if (player.Entity.Shape.Position.Y >= 1f) {
                GameRunning.GetInstance().ChangeLevel();
            } else {
                CollisionDetect(Entities, player);
            }
        }

        /// <summary>
        /// Handles the collision between entities and player
        /// </summary>
        /// <param name="Entities">
        /// List of entities
        /// </param>
        /// <param name="player">
        /// Player
        /// </param>
       


        private void CollisionDetect(EntityContainer<Entity> Entities, Player player) {
            foreach (Entity elm in Entities) {
                CollisionData col =
                    CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                        elm.Shape.AsDynamicShape());

               
                if (col.Collision) {
                    //Landing
                    if (col.CollisionDir == CollisionDirection.CollisionDirDown 
                         && player.Entity.Shape.AsDynamicShape().Direction.Y >= -0.005f 
                         
                         )
                    {
                        player.physics.IsGrounded = true;




                     //Collision with an obstacle. Taxi dies. 
                    } else {
                        Console.WriteLine(col.CollisionDir);
                        
                        Tuple<float, float> position =
                            new Tuple<float, float>(player.Entity.Shape.Position.X,
                                player.Entity.Shape.Position.Y);
                        Tuple<float, float> extent =
                            new Tuple<float, float>(player.Entity.Shape.Extent.X,
                                player.Entity.Shape.Extent.Y);

                        AddExplosion(position.Item1, position.Item2,
                            extent.Item1, extent.Item2);

                        player.Entity.Shape.AsDynamicShape().Direction=new Vec2F(0f,0f);
                        player.Entity.DeleteEntity();
                    }
                }
            }
        }
        
    }
    
    
}



/* public bool CollisionDetectCustomer(Entity customer, Player player) {
            CollisionData col =
                CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(),
                    customer.Shape.AsDynamicShape());
            Console.WriteLine("collided? {0}"
            ,col.Collision);



            return player.physics.IsGrounded; //&& col.Collision;
        }*/