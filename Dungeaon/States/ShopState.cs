using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Dungeaon.States
{
    class ShopState : State
    {
        private List<Component> components;

        private MainGameState.Item potion;
        private MainGameState.Item weapon;
        private MainGameState.Item defense;

        private Button item1Button;
        private Button item2Button;
        private Button item3Button;

        public ShopState(Game1 game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, State previousState) : base(game, graphicsDeviceManager, content, previousState)
        {
            Random rand = new Random();

            int potionIndex = rand.Next(MainGameState.postionList.Count);
            potion = MainGameState.postionList[potionIndex];

            int weaponIndex = rand.Next(MainGameState.weaponList.Count);
            weapon = MainGameState.weaponList[weaponIndex];

           int defenseIndex = rand.Next(MainGameState.defenseList.Count);
            defense = MainGameState.defenseList[defenseIndex];

            item1Button = new Button(new Vector2(530, 525))
            {
                texture = weapon.sprite,
                spriteScaleX = game.sword1.Width * 10,
                spriteScaleY = game.sword1.Height * 10,
            };

            item1Button.click += item1Button_Click;

            item2Button = new Button(new Vector2(700, 525))
            {
                texture = defense.sprite,
                spriteScaleX = game.shield1.Width * 10,
                spriteScaleY = game.shield1.Height * 10,
            };

            item2Button.click += item2Button_Click;

            item3Button = new Button(new Vector2(720, 200))
            {
                texture = potion.sprite,
                spriteScaleX = game.healthPostion.Width * 6,
                spriteScaleY = game.healthPostion.Height * 6,
            };

            item3Button.click += item3Button_Click;

            components = new List<Component>()
            {
                item1Button,
                item2Button,
                item3Button
            };

            components.AddRange(Player.inventorySlots);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            spriteBatch.Draw(game.devilShop, MainGameState.roomPos, null, Color.White, 0f, Vector2.Zero, 14f, SpriteEffects.None, 0);
            MainGameState.DrawUI(spriteBatch, game, graphicsDeviceManager);

            if (game.options.debugMode)
            {
                spriteBatch.DrawString(game.font, "X: " + mouseX + " Y: " + mouseY, new Vector2(0, 0), Color.Black, 0f, Vector2.Zero, 2, SpriteEffects.None, 0);
            }

            foreach (Component component in components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.DrawString(game.font, "Press 'Q' to exit!", new Vector2(1200, 895), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

            if (mouseRectangle.Intersects(item1Button.HitBoxRectangle))
                DrawToolTip(spriteBatch, weapon);
            else if (mouseRectangle.Intersects(item2Button.HitBoxRectangle))
                DrawToolTip(spriteBatch, defense);
            else if (mouseRectangle.Intersects(item3Button.HitBoxRectangle))
                DrawToolTip(spriteBatch, potion);

            foreach (Button slot in Player.inventorySlots)
            {
                if (mouseRectangle.Intersects(slot.HitBoxRectangle) && Player.inventory[slot].sprite != null)
                   MainGameState.DrawToolTip(spriteBatch, Player.inventory[slot], game);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        private Rectangle mouseRectangle;

        public override void Update(GameTime gameTime)
        {

            mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

            foreach (Component component in components)
            {
                component.Update(gameTime);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                game.ChangeState(previousState);
                MainGameState.player.position += new Vector2(0, 20);
            }
        }

        private void DrawToolTip(SpriteBatch spriteBatch, MainGameState.Item item)
        {
            int costWidth = (int)game.font.MeasureString("Cost: " + item.cost).X * 2;
            int nameWidth = (int)game.font.MeasureString(item.name).X * 2;
            int damageWidth = item.damage != 0 ? (int)game.font.MeasureString("Damage: " + item.damage).X * 2 : 0;
            int defenseWidth = item.defense != 0 ? (int)game.font.MeasureString("Defense: " + item.defense).X * 2 : 0;

            int costHeight = (int)game.font.MeasureString("Cost: " + item.cost).Y * 2;
            int nameHeight = (int)game.font.MeasureString(item.name).Y * 2;
            int damageHeight = item.damage != 0 ? (int)game.font.MeasureString("Damage: " + item.damage).Y * 2 : 0;
            int defenseHeight = item.defense != 0 ? (int)game.font.MeasureString("Defense: " + item.defense).Y * 2 : 0;

            int wCache1 = costWidth < nameWidth ? nameWidth : costWidth;
            int wCache2 = wCache1 < damageWidth ? damageWidth : wCache1;
            int wCache3 = wCache2 < defenseWidth ? defenseWidth : wCache2;


            Vector2 position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) + new Vector2(10, 10);

            Rectangle rect = new Rectangle((int)position.X, (int)position.Y, wCache3 + 10, nameHeight + costHeight + damageHeight + defenseHeight);

            spriteBatch.Draw(game.toolTip, rect, new Rectangle(0, 0, game.toolTip.Width, game.toolTip.Height), Color.White);
            spriteBatch.DrawString(game.font, item.name, position + new Vector2(5, 0), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(game.font, "Cost: " + item.cost, position + new Vector2(5, 30), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
           
            if (item.damage != 0)
                spriteBatch.DrawString(game.font, "Damage: " + item.damage, position + new Vector2(5, 60), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
          
            if (item.defense != 0)
                spriteBatch.DrawString(game.font, "Defense: " + item.defense, position + new Vector2(5, 60), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

        }

        private void item1Button_Click(object sender, EventArgs e)
        {
            if (weapon.cost <= Player.player_Money)
            {
                Player.player_Money -= weapon.cost;

                int i = 0;
                foreach (KeyValuePair<Button, MainGameState.Item> slot in Player.inventory)
                {
                    if (slot.Value.sprite == null)
                    {
                        Player.inventory[slot.Key] = weapon;
                        Player.inventorySlots[i].texture = weapon.sprite;
                        break;
                    }
                    i++;
                }
            }
        }

        private void item2Button_Click(object sender, EventArgs e)
        {
            if (defense.cost <= Player.player_Money)
            {
                Player.player_Money -= defense.cost;

                int i = 0;
                foreach (KeyValuePair<Button, MainGameState.Item> slot in Player.inventory)
                {
                    if (slot.Value.sprite == null)
                    {
                        Player.inventory[slot.Key] = defense;
                        Player.inventorySlots[i].texture = defense.sprite;
                        break;
                    }
                    i++;
                }
            }
        }

        private void item3Button_Click(object sender, EventArgs e)
        {
            if (potion.cost <= Player.player_Money)
            {
                Player.player_Money -= potion.cost;

                int i = 0;
                foreach (KeyValuePair<Button, MainGameState.Item> slot in Player.inventory)
                {
                    if (slot.Value.sprite == null)
                    {
                        Player.inventory[slot.Key] = potion;
                        Player.inventorySlots[i].texture = potion.sprite;
                        break;
                    }
                    i++;
                }
            }
        }
    }
}
