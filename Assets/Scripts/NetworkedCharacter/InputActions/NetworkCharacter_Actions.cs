using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSZZGame.Refactor;

namespace CSZZGame.Character
{
    public enum Action_Character_Type
    {
        NO_MOVE,
        MOVE,
        STEALTH,
        SHOOT,
        SKILL,
        LAUNCH_STRATEGEM
    }

    public interface IAction_Character
    {
        public Action_Character_Type identifier { get; }

        public bool IsActionAllowed(FSMState_Character_Type stateType);
    }

    public class Action_Character_Move : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.MOVE;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL          => true,
            FSMState_Character_Type.STEALTH_NORMAL  => true,
            FSMState_Character_Type.STEALTH_CLIMB   => true,
            _ => throw new System.NotImplementedException()
        };

        public Vector3 desiredMovement { get; private set; }

        public Action_Character_Move(Vector3 desiredMovement)
        {
            this.desiredMovement = desiredMovement;
        }
    }

    public class Action_Character_Stealth : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.STEALTH;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL          => startStealth,
            FSMState_Character_Type.STEALTH_NORMAL  => !startStealth,
            FSMState_Character_Type.STEALTH_CLIMB   => !startStealth,
            _ => throw new System.NotImplementedException()
        };

        public bool startStealth { get; private set; }

        public Action_Character_Stealth(bool isStart)
        {
            startStealth = isStart;
        }
    }

    public class Action_Character_Shoot : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.SHOOT;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL          => true,
            FSMState_Character_Type.STEALTH_NORMAL  => false,
            FSMState_Character_Type.STEALTH_CLIMB   => false,
            _ => throw new System.NotImplementedException()
        };
    }

    public class Action_Character_Skill : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.SKILL;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL          => true,
            FSMState_Character_Type.STEALTH_NORMAL  => false,
            FSMState_Character_Type.STEALTH_CLIMB   => false,
            _ => throw new System.NotImplementedException()
        };

        public int skillID { get; private set; }

        public Action_Character_Skill(int skillID)
        {
            this.skillID = skillID;
        }
    }

    public class Action_Character_LaunchStrategem : IAction_Character
    {
        public Action_Character_Type identifier => Action_Character_Type.LAUNCH_STRATEGEM;
        public bool IsActionAllowed(FSMState_Character_Type stateType) => stateType switch
        {
            FSMState_Character_Type.NORMAL => true,
            FSMState_Character_Type.STEALTH_NORMAL => false,
            FSMState_Character_Type.STEALTH_CLIMB => false,
            _ => throw new System.NotImplementedException()
        };
    }
}