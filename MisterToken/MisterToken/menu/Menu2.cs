﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MisterToken {
    public class Menu2 {
        public Menu2(PlayerIndex player, Action back) {
            selected = 0;
            this.offset = 0;
            this.player = player;
            this.back = back;
            items = new List<MenuItem>();
        }

        public void Add(String text, DefaultMenuItem.MenuAction action) {
            items.Add(new DefaultMenuItem(text, action));
        }

        public void AddLevel(int number, Level level, LevelMenuItem.MenuAction action) {
            items.Add(new LevelMenuItem(number, level, action));
        }

        public void AddMusic(string description, SoundHook song) {
            items.Add(new MusicMenuItem(description, song));
        }

        public void Update() {
            InputManager input = Global.Input;
            if (input.IsDown(PerPlayerBooleanInputHook.MENU_DOWN.ForPlayer(player)) ||
                input.IsDown(PerPlayerAnalogInputHook.MENU_DOWN.ForPlayer(player))) {
                int start = selected;
                //do {
                    selected = (selected + 1) % items.Count;
                //} while (selected != start && !items[selected].IsEnabled());
                offset = 1;
            }
            if (input.IsDown(PerPlayerBooleanInputHook.MENU_UP.ForPlayer(player)) ||
                input.IsDown(PerPlayerAnalogInputHook.MENU_UP.ForPlayer(player))) {
                int start = selected;
                //do {
                    selected--;
                    if (selected < 0) {
                        selected = items.Count - 1;
                    }
                //} while (selected != start && !items[selected].IsEnabled());
                offset = -1;
            }
            if (input.IsDown(PerPlayerBooleanInputHook.MENU_ENTER.ForPlayer(player)) ||
                input.IsDown(PerPlayerBooleanInputHook.ROTATE_RIGHT.ForPlayer(player))) {
                items[selected].OnEnter();
            }
            if (input.IsDown(PerPlayerBooleanInputHook.MENU_BACK.ForPlayer(player)) ||
                input.IsDown(PerPlayerBooleanInputHook.ROTATE_LEFT.ForPlayer(player))) {
                back();
            }

            if (offset < 0) {
                offset += 0.1;
                if (offset > 0) {
                    offset = 0;
                }
            }
            if (offset > 0) {
                offset -= 0.1;
                if (offset < 0) {
                    offset = 0;
                }
            }
        }

        public void SetSelected(int selected) {
            this.selected = selected;
            // Clear the current keyboard state.
            InputManager input = Global.Input;
            input.IsDown(PerPlayerBooleanInputHook.MENU_UP.ForPlayer(player));
            input.IsDown(PerPlayerBooleanInputHook.MENU_DOWN.ForPlayer(player));
        }

        public void Draw(int x, bool focused, SpriteBatch spriteBatch) {
            for (int i = -2; i < 4; ++i) {
                int item = selected + i;
                while (item < 0) {
                    item += items.Count;
                }
                while (item >= items.Count) {
                    item -= items.Count;
                }
                if (item >= 0 && item < items.Count) {
                    int y = 344 + (int)(80 * (i + offset));
                    items[item].Draw2(x, y, spriteBatch);
                    if (!focused) {
                        Rectangle area = new Rectangle();
                        area.X = x;
                        area.Y = y + 4;
                        area.Width = 588;
                        area.Height = 68;
                        Global.Sprites.DrawLayer(SpriteHook.MENU_DISABLED_LAYER, area, spriteBatch);
                    }
                }
            }
        }

        private PlayerIndex player;
        private Action back;
        private List<MenuItem> items;
        private int selected;
        private double offset;
    }
}