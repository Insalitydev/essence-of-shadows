using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities;
using EssenceShared.Entities.Player;
using EssenceShared.Entities.Projectiles;

namespace EssenceShared.Scenes {
    /** В этой сцене обрабатывается вся игровая логика на стороне сервера 
     * и на этой сцене рисуются все данные на стороне клиента */

    public class GameLayer: CCLayer {
        public List<Entity> entities = new List<Entity>();

        private Entity entity;
        public void AddEntity(EntityState es) {
            var textureName = es.TextureName;
            Log.Print(textureName);
            textureName = textureName.Replace("\\", "/").Split('/').Last();
            Log.Print(textureName);
            switch (textureName){
                case "Mystic":
                    entity = new Player(es.Id);
                    break;
                case "MysticProjectile":
                    entity = new MysticProjectile(es.Id, new CCPoint(0, 0));
                    break;
            }

            EntityState.AppendStateToEntity(entity, es);

            AddEntity(entity);
        }

        public void AddEntity(Entity e) {
            Log.Print("ADDING" + EntityState.ParseEntity(e).Serialize());
            entities.Add(e);
            AddChild(e);
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(UpdateDebug, 2);
        }


        private void UpdateDebug(float dt) {
            Log.Print(GetGameState().Serialize());
        }
        public override void Update(float dt) {
            base.Update(dt);
        }

        public GameState GetGameState() {
            var gs = new GameState();

            foreach (Entity entity in entities){
                gs.entities.Add(EntityState.ParseEntity(entity));
            }

            return gs;
        }

        /** Клиентское */

        public void AppendGameState(GameState gs, string playerId) {
            /** Updating entities */
            foreach (EntityState entity in gs.entities){
                Log.Print("SHOULD UPDATE" + entity.Id);
                int index = entities.FindIndex(x=>x.Id == entity.Id);
                if (index != -1){
                    if (entity.Id != playerId){
                        entities[index].AppendState(entity);
                    }
                }
                else{
                    AddEntity(entity);
                }
            }
        }


        /** серверное */

        public void UpdateEntity(EntityState es) {
            int index = entities.FindIndex(x=>x.Id == es.Id);
            if (index == -1){
                AddEntity(es);
            }
            else{
                EntityState.AppendStateToEntity(entities[index], es);
//                entities[index].PositionX = e.PositionX;
//                entities[index].PositionY = e.PositionY;
            }
        }

        public Entity FindEntityById(string id) {
            int result = entities.FindIndex(x=>x.Id == id);
            if (result == -1){
                return null;
            }
            return entities[result];
        }
    }
}