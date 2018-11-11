using Terraria;
using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria.ModLoader.IO;
using Terraria.UI;
using System.Collections.Generic;

namespace PressF11
{
    class F11Player : ModPlayer
    {
        private bool hideMode = true;
        private int slot = 1;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (PressF11.SwitchSlots.JustPressed && player.whoAmI == Main.myPlayer)
            {
                for (int i = 0; i < 10; i++)
                {
                    Item tmp = player.inventory[slot * 10 + i];
                    player.inventory[slot * 10 + i] = player.inventory[i];
                    player.inventory[i] = tmp;
                    Main.PlaySound(7, -1, -1, 1, 1f, 0f);
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(5, -1, -1, null, Main.myPlayer, (float)(i), (float)Main.player[Main.myPlayer].inventory[i].prefix, 0f, 0, 0, 0);
                        NetMessage.SendData(5, -1, -1, null, Main.myPlayer, (float)(slot * 10 + i), (float)Main.player[Main.myPlayer].inventory[slot * 10 + i].prefix, 0f, 0, 0, 0);
                    }
                }
                slot++;
                if (slot > 4)
                {
                    slot = 1;
                }
            }
            if (PressF11.SwapVertical.JustPressed && player.whoAmI == Main.myPlayer)
            {
                Item tmp = player.inventory[player.selectedItem];
                player.inventory[player.selectedItem] = player.inventory[player.selectedItem + 10];
                player.inventory[player.selectedItem + 10] = tmp;
                Main.PlaySound(7, -1, -1, 1, 1f, 0f);
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(5, -1, -1, null, Main.myPlayer, (float)(player.selectedItem), (float)Main.player[Main.myPlayer].inventory[player.selectedItem].prefix, 0f, 0, 0, 0);
                    NetMessage.SendData(5, -1, -1, null, Main.myPlayer, (float)(player.selectedItem + 10), (float)Main.player[Main.myPlayer].inventory[player.selectedItem + 10].prefix, 0f, 0, 0, 0);
                }
            }
            if (PressF11.SwapHorizontal.JustPressed && player.whoAmI == Main.myPlayer)
            {
                Item tmp = player.inventory[player.selectedItem];
                player.inventory[player.selectedItem] = player.inventory[(player.selectedItem + 1) % 10];
                player.inventory[(player.selectedItem + 1) % 10] = tmp;
                Main.PlaySound(7, -1, -1, 1, 1f, 0f);
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(5, -1, -1, null, Main.myPlayer, (float)(player.selectedItem), (float)Main.player[Main.myPlayer].inventory[player.selectedItem].prefix, 0f, 0, 0, 0);
                    NetMessage.SendData(5, -1, -1, null, Main.myPlayer, (float)((player.selectedItem + 1) % 10), (float)Main.player[Main.myPlayer].inventory[(player.selectedItem + 1) % 10].prefix, 0f, 0, 0, 0);
                }
            }
            if (PressF11.LootAll.JustPressed && player.whoAmI == Main.myPlayer)
            {
                if (player.chest != -1)
                {
                    ChestUI.LootAll();
                }
            }
            //random craft
            if (PressF11.CraftRandom.JustPressed && player.whoAmI == Main.myPlayer)
            {
                Recipe.FindRecipes();
                if (Main.numAvailableRecipes >= 1)
                {
                    Main.focusRecipe = Main.rand.Next(Main.numAvailableRecipes);
                    Recipe r = Main.recipe[Main.availableRecipe[Main.focusRecipe]];
                    r.Create();
                    Main.PlaySound(7, -1, -1, 1, 1f, 0f);
                    Item.NewItem(player.position + player.Size / 2, r.createItem.type, r.createItem.stack, false, r.createItem.prefix, true);
                }
            }
        }

        public override void PostUpdate()
        {
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.F11))
            {
                hideMode = Main.hideUI;
            }
            if (Main.hideUI)
            {
                if (PlayerInput.Triggers.JustPressed.Inventory)
                {
                    IngameOptions.Open();
                    Main.hideUI = false;
                }
                #region talkNPC
                Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int)((float)Main.mouseX + Main.screenPosition.X), (int)((float)Main.mouseY + Main.screenPosition.Y), 1, 1);
                if (Main.player[Main.myPlayer].gravDir == -1f)
                {
                    rectangle.Y = (int)Main.screenPosition.Y + Main.screenHeight - Main.mouseY;
                }
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active)
                    {
                        int type = Main.npc[k].type;
                        Main.instance.LoadNPC(type);
                        Microsoft.Xna.Framework.Rectangle value3 = new Microsoft.Xna.Framework.Rectangle((int)Main.npc[k].Bottom.X - Main.npc[k].frame.Width / 2, (int)Main.npc[k].Bottom.Y - Main.npc[k].frame.Height, Main.npc[k].frame.Width, Main.npc[k].frame.Height);
                        if (Main.npc[k].type >= 87 && Main.npc[k].type <= 92)
                        {
                            value3 = new Microsoft.Xna.Framework.Rectangle((int)((double)Main.npc[k].position.X + (double)Main.npc[k].width * 0.5 - 32.0), (int)((double)Main.npc[k].position.Y + (double)Main.npc[k].height * 0.5 - 32.0), 64, 64);
                        }
                        bool flag = rectangle.Intersects(value3);
                        if (flag && ((Main.npc[k].type != 85 && Main.npc[k].type != 341 && Main.npc[k].aiStyle != 87) || Main.npc[k].ai[0] != 0f) && Main.npc[k].type != 488)
                        {
                            bool flag3 = Main.SmartInteractShowingGenuine && Main.SmartInteractNPC == k;
                            if (Main.npc[k].townNPC || Main.npc[k].type == 105 || Main.npc[k].type == 106 || Main.npc[k].type == 123 || Main.npc[k].type == 354 || Main.npc[k].type == 376 || Main.npc[k].type == 579 || Main.npc[k].type == 453)
                            {
                                Microsoft.Xna.Framework.Rectangle rectangle2 = new Microsoft.Xna.Framework.Rectangle((int)(Main.player[Main.myPlayer].position.X + (float)(Main.player[Main.myPlayer].width / 2) - (float)(Player.tileRangeX * 16)), (int)(Main.player[Main.myPlayer].position.Y + (float)(Main.player[Main.myPlayer].height / 2) - (float)(Player.tileRangeY * 16)), Player.tileRangeX * 16 * 2, Player.tileRangeY * 16 * 2);
                                Microsoft.Xna.Framework.Rectangle value4 = new Microsoft.Xna.Framework.Rectangle((int)Main.npc[k].position.X, (int)Main.npc[k].position.Y, Main.npc[k].width, Main.npc[k].height);
                                if (rectangle2.Intersects(value4))
                                {
                                    flag3 = true;
                                }
                            }
                            if (Main.player[Main.myPlayer].ownedProjectileCounts[651] > 0)
                            {
                                flag3 = false;
                            }
                            if (flag3 && !Main.player[Main.myPlayer].dead)
                            {
                                PlayerInput.SetZoom_MouseInWorld();
                                Main.HoveringOverAnNPC = true;
                                Main.instance.currentNPCShowingChatBubble = k;
                                if (Main.mouseRight && Main.mouseRightRelease)
                                {
                                    Main.npcChatRelease = false;
                                    if (PlayerInput.UsingGamepad)
                                    {
                                        Main.player[Main.myPlayer].releaseInventory = false;
                                    }
                                    if (Main.player[Main.myPlayer].talkNPC != k)
                                    {
                                        Main.CancelHairWindow();
                                        Main.npcShop = 0;
                                        Main.InGuideCraftMenu = false;
                                        Main.player[Main.myPlayer].dropItemCheck();
                                        Main.npcChatCornerItem = 0;
                                        Main.player[Main.myPlayer].sign = -1;
                                        Main.editSign = false;
                                        Main.player[Main.myPlayer].talkNPC = k;
                                        Main.playerInventory = false;
                                        Main.player[Main.myPlayer].chest = -1;
                                        Recipe.FindRecipes();
                                        Main.npcChatText = Main.npc[k].GetChat();
                                        break;
                                        //Main.PlaySound(24, -1, -1, 1, 1f, 0f);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            if (Main.playerInventory && hideMode)
            {
                Main.playerInventory = false;
                Main.hideUI = true;
            }
            //random buy
            if (player.talkNPC >= 0)
            {
                setNPCShop();
                if (Main.npcShop > 0)
                {
                    Chest.SetupTravelShop();
                    Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                    List<int> tryBuyIndex = new List<int>();
                    Item[] shopItem = Main.instance.shop[Main.npcShop].item;
                    int length = shopItem.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (shopItem[i].type != 0 && shopItem[i].stack != 0)
                        {
                            tryBuyIndex.Add(i);
                        }
                    }
                    int n = tryBuyIndex.Count;
                    while (n > 1)
                    {
                        n--;
                        int k = Main.rand.Next(n + 1);
                        int value = tryBuyIndex[k];
                        tryBuyIndex[k] = tryBuyIndex[n];
                        tryBuyIndex[n] = value;
                    }
                    for (int i = 0; i < n; i++)
                    {
                        if (player.BuyItem(shopItem[tryBuyIndex[i]].GetStoreValue(), shopItem[tryBuyIndex[i]].shopSpecialCurrency))
                        {
                            Item.NewItem(player.position + player.Size / 2, shopItem[tryBuyIndex[i]].type, shopItem[tryBuyIndex[i]].stack, false, shopItem[tryBuyIndex[i]].prefix, true);
                            player.talkNPC = -1;
                            Main.npcShop = 0;
                            Main.PlaySound(18);
                            break;
                        }
                    }
                }

            }
        }

        public override void OnEnterWorld(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                if (Main.releaseUI)
                {
                    Main.hideUI = true;
                    Main.releaseUI = true;
                }
            }
        }

        private void setNPCShop()
        {
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 17)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 1;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 19)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 2;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 124)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 8;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 142)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 9;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 353)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 18;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 368)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 19;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 453)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 20;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 37)
            {
                if (Main.netMode == 0)
                {
                    NPC.SpawnSkeletron();
                }
                else
                {
                    NetMessage.SendData(51, -1, -1, null, Main.myPlayer, 1f, 0f, 0f, 0, 0, 0);
                }
                Main.npcChatText = "";
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 20)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 3;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 38)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 4;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 54)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 5;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 107)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 6;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 108)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 7;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 160)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 10;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 178)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 11;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 207)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 12;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 208)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 13;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 209)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 14;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 227)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 15;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 228)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 16;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 229)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 17;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 22)
            {
                //Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                return;
            }
            if (Main.npc[Main.player[Main.myPlayer].talkNPC].type == 550)
            {
                Main.playerInventory = true;
                Main.npcChatText = "";
                Main.npcShop = 21;
                Main.instance.shop[Main.npcShop].SetupShop(Main.npcShop);
                return;
            }
        }
    }
}
