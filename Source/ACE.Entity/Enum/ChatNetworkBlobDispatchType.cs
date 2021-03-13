namespace ACE.Entity.Enum
{
    public enum ChatNetworkBlobDispatchType
    {
        ASYNCMETHOD_UNKNOWN                 = 0,
        ASYNCMETHOD_SENDTOROOMBYNAME        = 1,
        ASYNCMETHOD_SENDTOROOMBYID          = 2,
        ASYNCMETHOD_CREATEROOM              = 3,
        ASYNCMETHOD_INVITECLIENTTOROOMBYID  = 4,
        ASYNCMETHOD_EJECTCLIENTFROMROOMBYID = 5
    }
}
