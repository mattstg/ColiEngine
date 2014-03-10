using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Msquared.ColiEngine.EntitySystem
{
    class tempBodyStats
    {

        //base stats of all bodies?

        int strength;
        int endurance;
        int intellegence;
        int spirit;
        int agility;
        int dexterity;
        
        //constants
        float etransferRateMult = (float)0.5;
        float maxEnergyMult = (float)5.0;
        float eRegenMult = (float)0.1;

        float hpMult = 10;
        float hpRegenMult = (float)0.1;
        float armorMult = 1;
        float movementsEnergyCostMult = 1;
        float foceOfMovementMult = 1;
        float FallDmgReductionMult = 1;
        float bonusJumpForceMult = 1;
        float cooldownReductionMult = 1;
        float channelSpeedMult = 1;
        float physicaldmgMult = 1;
        float digEfficiencyMult = 1;
        float dmgMagicMult = 1;
        
        public tempBodyStats(int s, int e, int i, int sp, int a, int d){
            strength = s;
            endurance = e;
            intellegence = i;
            spirit = sp;
            agility = a;
            dexterity = d;

            //energy stuff
            float baseBodyPartEnergyTransferRate = (intellegence + spirit) * etransferRateMult; //the rate of transfer between body parts is affected by this
            float energyMax = (intellegence + spirit) * maxEnergyMult; //maximum energy storage of body part
            float energyRegen = spirit*eRegenMult;//rate at which total energy is regenerated
            float channelSpeed = (intellegence * dexterity) * channelSpeedMult;//a value which is multiplied to the total channel time of a spell (which decreases it)
            float cooldownReduction = (agility + dexterity + intellegence) * cooldownReductionMult; //cooldown between actions is negatively affected by this stat?
        
            //health & armor?
            float hpMax = endurance * hpMult; //total hp of body
            float hpRegen = (endurance+dexterity) * hpRegenMult; //the regeneration rate of total hp
            float armor = endurance*armorMult; //decreases damage taken by some calculation with material type and damage type (as well as armor)

            //Movement
            float energyCostMovement = (agility + dexterity) * movementsEnergyCostMult;//this should decreased the amount of energy it costs to move body
            float bonusForceOfMovement = (strength + agility) * foceOfMovementMult; //this should just be a multiplier on the base force applied when moving left,right,down?
            float bonusJumpForce = (agility + strength) * bonusJumpForceMult; //this multiplied with weight of body should make up jump force (and therefore jump high)
            float FallDmgReduction = (agility + dexterity) * FallDmgReductionMult;//simple multiplier which slightly decreases damage
            float digEfficiency = strength * digEfficiencyMult; //this should make highStrength chars be able to dig very easily through dirt

            //
            float dmgPhysical = (strength) * physicaldmgMult;
            float dmgMagic = intellegence * dmgMagicMult;





        }











    }
}
