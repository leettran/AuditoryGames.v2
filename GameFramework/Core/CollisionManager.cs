using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace LSRI.AuditoryGames.GameFramework
{
    public class CollisionManager
    {
        protected static CollisionManager instance = null;
        protected Dictionary<string, List<string>> collisionMap = new Dictionary<string, List<string>>();

        public static CollisionManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new CollisionManager();
                return instance;
            }
        }

        protected CollisionManager()
        {

        }

        public void addCollisionMapping(string type1, string type2)
        {
            if (!collisionMap.ContainsKey(type1))
                collisionMap.Add(type1, new List<string>());
            if (!collisionMap.ContainsKey(type2))
                collisionMap.Add(type2, new List<string>());

            if (!collisionMap[type1].Contains(type2))
                collisionMap[type1].Add(type2);
            if (!collisionMap[type2].Contains(type1))
                collisionMap[type2].Add(type1);
            
        }

        public void enterFrame(double dt)
        {
            for (int i = 0; i < GameObject.gameObjects.Count; ++i)
			{
				GameObject gameObjectI = GameObject.gameObjects[i];
							
				for (int j = i + 1; j < GameObject.gameObjects.Count; ++j)
				{
			        GameObject gameObjectJ = GameObject.gameObjects[j];
					
			        // early out for non-colliders
			        bool collisionNameNotNothing = gameObjectI.CollisionName != CollisionIdentifiers.NONE &&
                        gameObjectJ.CollisionName != CollisionIdentifiers.NONE;
			        // objects can still exist in the baseObjects collection after being disposed, so check
			        bool bothInUse = gameObjectI.InUse && gameObjectJ.InUse;
			        // make sure we have an entry in the collisionMap
			        bool collisionMapEntryExists = collisionMap.ContainsKey(gameObjectI.CollisionName);
			        // make sure the two objects are set to collide
                    bool testForCollision = collisionMapEntryExists && collisionMap[gameObjectI.CollisionName].Contains(gameObjectJ.CollisionName);
					
			        if ( collisionNameNotNothing &&					
				         bothInUse &&		
				         collisionMapEntryExists &&
				         testForCollision)
			        {
                        if (gameObjectJ.CollisionName == CollisionIdentifiers.PLAYERWEAPON)
                        {
                            if (MathUtil.PointInRect(gameObjectJ.Rect, gameObjectI.Rect))
                            {
                                gameObjectI.collision(gameObjectJ);
                                gameObjectJ.collision(gameObjectI);
                            }
                        }
				        else if (MathUtil.RectIntersect(gameObjectI.Rect, gameObjectJ.Rect))
				        {
                            gameObjectI.collision(gameObjectJ);
					        gameObjectJ.collision(gameObjectI);
				        }
			        }					
				}
			}
        }
        

    }
}
