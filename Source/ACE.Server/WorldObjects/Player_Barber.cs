using System.Collections.Generic;

using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void HandleActionFinishBarber(ClientMessage message)
        {
            if (!BarberActive) return;

            // Read the payload sent from the client...
            var requestedPaletteBaseId = message.Payload.ReadUInt32();
            var requestedHeadObjectDID = message.Payload.ReadUInt32();
            var requestedCharacterHairTexture = message.Payload.ReadUInt32();
            var requestedCharacterDefaultHairTexture = message.Payload.ReadUInt32();
            var requestedEyesTextureDID = message.Payload.ReadUInt32();
            var requestedDefaultEyesTextureDID = message.Payload.ReadUInt32();
            var requestedNoseTextureDID = message.Payload.ReadUInt32();
            var requestedDefaultNoseTextureDID = message.Payload.ReadUInt32();
            var requestedMouthTextureDID = message.Payload.ReadUInt32();
            var requestedDefaultMouthTextureDID = message.Payload.ReadUInt32();
            var requestedSkinPaletteDID = message.Payload.ReadUInt32();
            var requestedHairPaletteDID = message.Payload.ReadUInt32();
            var requestedEyesPaletteDID = message.Payload.ReadUInt32();
            var requestedSetupTableId = message.Payload.ReadUInt32();

            uint option_bound = message.Payload.ReadUInt32(); // Supress Levitation - Empyrean Only
            uint option_unk = message.Payload.ReadUInt32(); // Unknown - Possibly set aside for future use?

            //var debugMsg = "Barber Change Request:";
            //debugMsg += System.Environment.NewLine + $"PaletteBaseId:                0x{PaletteBaseId:X8} to 0x{requestedPaletteBaseId:X8}";
            //debugMsg += System.Environment.NewLine + $"HeadObjectDID:                0x{HeadObjectDID ?? 0:X8} to 0x{requestedHeadObjectDID:X8}";
            //debugMsg += System.Environment.NewLine + $"Character.HairTexture:        0x{Character.HairTexture:X8} to 0x{requestedCharacterHairTexture:X8}";
            //debugMsg += System.Environment.NewLine + $"Character.DefaultHairTexture: 0x{Character.DefaultHairTexture:X8} to 0x{requestedCharacterDefaultHairTexture:X8}";
            //debugMsg += System.Environment.NewLine + $"EyesTextureDID:               0x{EyesTextureDID:X8} to 0x{requestedEyesTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"DefaultEyesTextureDID:        0x{DefaultEyesTextureDID:X8} to 0x{requestedDefaultEyesTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"NoseTextureDID:               0x{NoseTextureDID:X8} to 0x{requestedNoseTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"DefaultNoseTextureDID:        0x{DefaultNoseTextureDID:X8} to 0x{requestedDefaultNoseTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"MouthTextureDID:              0x{MouthTextureDID:X8} to 0x{requestedMouthTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"DefaultMouthTextureDID:       0x{DefaultMouthTextureDID:X8} to 0x{requestedDefaultMouthTextureDID:X8}";
            //debugMsg += System.Environment.NewLine + $"SkinPaletteDID:               0x{SkinPaletteDID:X8} to 0x{requestedSkinPaletteDID:X8}";
            //debugMsg += System.Environment.NewLine + $"HairPaletteDID:               0x{HairPaletteDID:X8} to 0x{requestedHairPaletteDID:X8}";
            //debugMsg += System.Environment.NewLine + $"EyesPaletteDID:               0x{EyesPaletteDID:X8} to 0x{requestedEyesPaletteDID:X8}";
            //debugMsg += System.Environment.NewLine + $"SetupTableId:                 0x{SetupTableId:X8} to 0x{requestedSetupTableId:X8}";
            //debugMsg += System.Environment.NewLine + $"Option Bound:                 {option_bound}";
            //debugMsg += System.Environment.NewLine + $"Option Unknown:               {option_unk}" + System.Environment.NewLine;
            //Console.WriteLine(debugMsg);

            var heritageGroup = DatManager.PortalDat.CharGen.HeritageGroups[(uint)Heritage];
            var sex = heritageGroup.Genders[(int)Gender];

            var validPaletteBase = sex.BasePalette == requestedPaletteBaseId;

            var validHeadObject = ValidateHairStyle(requestedHeadObjectDID, sex.HairStyleList, out var validatedHairStyle);

            var validEyesTexture = ValidateEyeTexture(requestedEyesTextureDID, sex.EyeStripList, isBald: validatedHairStyle?.Bald ?? false);

            var validDefaultEyesTexture = ValidateEyeTexture(requestedDefaultEyesTextureDID, sex.EyeStripList, compareOldTexture: true, isBald: validatedHairStyle?.Bald ?? false);

            var validNoseTexture = ValidateFaceTexture(requestedNoseTextureDID, sex.NoseStripList);

            var validDefaultNoseTexture = ValidateFaceTexture(requestedDefaultNoseTextureDID, sex.NoseStripList, compareOldTexture: true);

            var validMouthTexture = ValidateFaceTexture(requestedMouthTextureDID, sex.MouthStripList);

            var validDefaultMouthTexture = ValidateFaceTexture(requestedDefaultMouthTextureDID, sex.MouthStripList, compareOldTexture: true);

            var validCharacterHairTexture = ValidateHairTexture(requestedCharacterHairTexture, validatedHairStyle);

            var validCharacterDefaultHairTexture = ValidateHairTexture(requestedCharacterDefaultHairTexture, validatedHairStyle, compareOldTexture: true);

            var validSkinPalette = ValidateSkinPalette(requestedSkinPaletteDID, sex.SkinPalSet);

            var validEyesPalette = ValidateEyesPalette(requestedEyesPaletteDID, sex.EyeColorList);

            var validHairPalette = ValidateHairPalette(requestedHairPaletteDID, sex.HairColorList);

            var validSetupTable = ValidateSetupTable(requestedSetupTableId, heritageGroup, sex, validatedHairStyle);

            var validChangeRequested = validPaletteBase && validHeadObject
                && validEyesTexture && validDefaultEyesTexture && validEyesPalette
                && validNoseTexture && validDefaultNoseTexture
                && validMouthTexture && validDefaultMouthTexture
                && validCharacterHairTexture && validCharacterDefaultHairTexture && validHairPalette
                && validSkinPalette && validSetupTable;

            //var previousSetupTableId = SetupTableId;
            //var previousMotionTableId = MotionTableId;

            if (!validChangeRequested)
            {
                // Don't know what, if anything, to send to player, so silently failing for now.
                //SendTransientError("The barber cannot do what you requested.");
                BarberActive = false;
                return;
            }
            else
            {
                if (requestedPaletteBaseId > 0)
                    PaletteBaseId = requestedPaletteBaseId;

                if (requestedHeadObjectDID > 0)
                    HeadObjectDID = requestedHeadObjectDID;

                if (requestedCharacterHairTexture > 0)
                {
                    Character.HairTexture = requestedCharacterHairTexture;
                    CharacterChangesDetected = true;
                }
                if (requestedCharacterDefaultHairTexture > 0)
                {
                    Character.DefaultHairTexture = requestedCharacterDefaultHairTexture;
                    CharacterChangesDetected = true;
                }

                if (requestedEyesTextureDID > 0)
                    EyesTextureDID = requestedEyesTextureDID;
                if (requestedDefaultEyesTextureDID > 0)
                    DefaultEyesTextureDID = requestedDefaultEyesTextureDID;

                if (requestedNoseTextureDID > 0)
                    NoseTextureDID = requestedNoseTextureDID;
                if (requestedDefaultNoseTextureDID > 0)
                    DefaultNoseTextureDID = requestedDefaultNoseTextureDID;

                if (requestedMouthTextureDID > 0)
                    MouthTextureDID = requestedMouthTextureDID;
                if (requestedDefaultMouthTextureDID > 0)
                    DefaultMouthTextureDID = requestedDefaultMouthTextureDID;

                if (requestedSkinPaletteDID > 0)
                    SkinPaletteDID = requestedSkinPaletteDID;

                if (requestedHairPaletteDID > 0)
                    HairPaletteDID = requestedHairPaletteDID;

                if (requestedEyesPaletteDID > 0)
                    EyesPaletteDID = requestedEyesPaletteDID;

                if (requestedSetupTableId > 0)
                    SetupTableId = requestedSetupTableId;
            }

            // Check if Character is Empyrean, and if we need to set/change/send new motion table
            if (Heritage == (int)HeritageGroup.Empyrean)
            {
                // These are the motion tables for Empyrean float and not-float (one for each gender). They are hard-coded into the client.
                const uint EmpyreanMaleFloatMotionDID = 0x0900020Bu;
                const uint EmpyreanFemaleFloatMotionDID = 0x0900020Au;
                const uint EmpyreanMaleMotionDID = 0x0900020Eu;
                const uint EmpyreanFemaleMotionDID = 0x0900020Du;

                // Check for the Levitation option for Empyrean. Shadow crown and Undead flames are handled by client.
                if (Gender == (int)ACE.Entity.Enum.Gender.Male) // Male
                {
                    if (option_bound == 1 && MotionTableId != EmpyreanMaleMotionDID)
                    {
                        MotionTableId = EmpyreanMaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, MotionTableId));
                    }
                    else if (option_bound == 0 && MotionTableId != EmpyreanMaleFloatMotionDID)
                    {
                        MotionTableId = EmpyreanMaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, MotionTableId));
                    }
                }
                else // Female
                {
                    if (option_bound == 1 && MotionTableId != EmpyreanFemaleMotionDID)
                    {
                        MotionTableId = EmpyreanFemaleMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, MotionTableId));
                    }
                    else if (option_bound == 0 && MotionTableId != EmpyreanFemaleFloatMotionDID)
                    {
                        MotionTableId = EmpyreanFemaleFloatMotionDID;
                        Session.Network.EnqueueSend(new GameMessagePrivateUpdateDataID(this, PropertyDataId.MotionTable, MotionTableId));
                    }
                }
            }


            // Broadcast updated character appearance
            EnqueueBroadcast(new GameMessageObjDescEvent(this));

            // The following code provides for updated [no]flame/[no]crown/[no]hover setups to be seen by others immediately without need for the player to log out/in
            // however it creates movement desync which must be the result of using UpdateObject message.
            // We don't have (that I could find) pcaps of barber changes from observer perspective so I'm uncertain if the visual desync between setup changes was resolved by some other method
            //
            //if (previousSetupTableId != SetupTableId || previousMotionTableId != MotionTableId)
            //{
            //    EnqueueBroadcast(false, new GameMessageUpdateObject(this));
            //    Session.Network.EnqueueSend(new GameMessageObjDescEvent(this));
            //}
            //else
            //    EnqueueBroadcast(new GameMessageObjDescEvent(this));

            BarberActive = false;
        }

        private bool ValidateSetupTable(uint requestedSetupTableId, HeritageGroupCG heritageGroup, SexCG sex, HairStyleCG validHairStyle)
        {
            if (validHairStyle?.AlternateSetup > 0 && validHairStyle?.AlternateSetup == requestedSetupTableId)
                return true;
            else if (sex.SetupID == requestedSetupTableId)
                return true;
            //else if (heritageGroup.SetupID == requestedSetupTableId)
            //    return true;

            if (ValidHeritageSetups.TryGetValue((HeritageGroup)Heritage, out var genders) && genders.TryGetValue((Gender)Gender, out var gender) && gender.Contains(requestedSetupTableId))
                return true;

            return false;
        }

        private Dictionary<HeritageGroup, Dictionary<Gender, List<uint>>> ValidHeritageSetups = new()
        {
            { HeritageGroup.Shadowbound, new () {
                { ACE.Entity.Enum.Gender.Male, new() {
                    (uint)SetupConst.UmbraenMaleCrown, (uint)SetupConst.UmbraenMaleNoCrown }
                },
                { ACE.Entity.Enum.Gender.Female, new() {
                    (uint)SetupConst.UmbraenFemaleCrown, (uint)SetupConst.UmbraenFemaleNoCrown } }
                }
            },
            { HeritageGroup.Penumbraen, new () {
                { ACE.Entity.Enum.Gender.Male, new() {
                    (uint)SetupConst.PenumbraenMaleCrown, (uint)SetupConst.PenumbraenMaleNoCrown }
                },
                { ACE.Entity.Enum.Gender.Female, new() {
                    (uint)SetupConst.PenumbraenFemaleCrown, (uint)SetupConst.PenumbraenFemaleNoCrown } }
                }
            },
            { HeritageGroup.Undead, new () {
                { ACE.Entity.Enum.Gender.Male, new() {
                    (uint)SetupConst.UndeadMaleSkeleton, (uint)SetupConst.UndeadMaleSkeletonNoFlame,
                    (uint)SetupConst.UndeadMaleZombie, (uint)SetupConst.UndeadMaleZombieNoFlame }
                },
                { ACE.Entity.Enum.Gender.Female, new() {
                    (uint)SetupConst.UndeadFemaleSkeleton, (uint)SetupConst.UndeadFemaleSkeletonNoFlame,
                    (uint)SetupConst.UndeadFemaleZombie, (uint)SetupConst.UndeadFemaleZombieNoFlame } }
                }
            }
        };

        private bool ValidateHairStyle(uint requestedHeadObjectDID, List<HairStyleCG> hairStyleList, out HairStyleCG validHairStyle)
        {
            validHairStyle = null;

            //var validHairStyles = hairStyleList.Where(h => h.ObjDesc.AnimPartChanges[0].PartID == requestedHeadObjectDID).ToList();

            if (requestedHeadObjectDID == 0)
            {
                if (Heritage == (int)HeritageGroup.Gearknight || Heritage == (int)HeritageGroup.Olthoi || Heritage == (int)HeritageGroup.OlthoiAcid)
                    return true;
            }

            foreach (var hairStyle in hairStyleList)
            {
                foreach (var animPartChange in hairStyle.ObjDesc.AnimPartChanges)
                {
                    if (animPartChange.PartID == requestedHeadObjectDID)
                    {
                        validHairStyle = hairStyle;
                        return true;
                    }
                }
            }    

            return false;
        }

        private bool ValidateEyeTexture(uint requestedEyesTextureDID, List<EyeStripCG> eyeStripList, bool compareOldTexture = false, bool isBald = false)
        {
            foreach (var eyeStrip in eyeStripList)
            {
                if (isBald)
                {
                    foreach (var textureMapChange in eyeStrip.ObjDescBald.TextureChanges)
                    {
                        if (compareOldTexture && textureMapChange.OldTexture == requestedEyesTextureDID)
                            return true;
                        else if (!compareOldTexture && textureMapChange.NewTexture == requestedEyesTextureDID)
                            return true;
                    }
                }
                else
                {
                    foreach (var textureMapChange in eyeStrip.ObjDesc.TextureChanges)
                    {
                        if (compareOldTexture && textureMapChange.OldTexture == requestedEyesTextureDID)
                            return true;
                        else if (!compareOldTexture && textureMapChange.NewTexture == requestedEyesTextureDID)
                            return true;
                    }
                }
            }

            return false;
        }

        private bool ValidateFaceTexture(uint requestedFaceTextureDID, List<FaceStripCG> faceStripList, bool compareOldTexture = false)
        {
            foreach (var faceStrip in faceStripList)
            {
                foreach (var textureMapChange in faceStrip.ObjDesc.TextureChanges)
                {
                    if (compareOldTexture && textureMapChange.OldTexture == requestedFaceTextureDID)
                        return true;
                    else if (!compareOldTexture && textureMapChange.NewTexture == requestedFaceTextureDID)
                        return true;
                }
            }

            return false;
        }

        private bool ValidateHairTexture(uint requestedHairTextureDID, HairStyleCG hairStyle, bool compareOldTexture = false)
        {
            if (requestedHairTextureDID == 0)
            {
                if (Heritage == (int)HeritageGroup.Gearknight || Heritage == (int)HeritageGroup.Olthoi || Heritage == (int)HeritageGroup.OlthoiAcid)
                    return true;
            }
            else if (hairStyle == null)
                return false;

            foreach (var textureMapChange in hairStyle.ObjDesc.TextureChanges)
            {
                if (compareOldTexture && textureMapChange.OldTexture == requestedHairTextureDID)
                    return true;
                else if (!compareOldTexture && textureMapChange.NewTexture == requestedHairTextureDID)
                    return true;
            }

            return false;
        }

        private bool ValidateSkinPalette(uint requestedSkinPaletteDID, uint paletteSet)
        {
            var skinPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(paletteSet);
            if (skinPalSet.PaletteList.Contains(requestedSkinPaletteDID))
                return true;

            return false;
        }

        private bool ValidateEyesPalette(uint requestedEyesPaletteDID, List<uint> eyeColorList)
        {
            if (eyeColorList.Contains(requestedEyesPaletteDID))
                return true;

            return false;
        }

        private bool ValidateHairPalette(uint requestedHairPaletteDID, List<uint> hairColorList)
        {
            foreach (var hairPalette in hairColorList)
            {
                var hairPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(hairPalette);
                if (hairPalSet.PaletteList.Contains(requestedHairPaletteDID))
                    return true;
            }

            return false;
        }
    }
}
