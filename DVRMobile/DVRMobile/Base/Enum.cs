namespace DVRMobile.Base
{
    public enum TOAST_LENGTH
    {
        LONG,
        SHORT
    }

    public enum ENUM_DIVISIONS
    {
        NONE = 0,
        DIVISION_1 = 1,
        DIVISION_4 = 4,
        DIVISION_9 = 9,
        DIVISION_16 = 16,
        DIVISION_36 = 36,
        DIVISION_64 = 64
    }

    public enum ENUM_MSG_ID
    {
        NONE = 0,
        MSG_ID_OK,
        AUTH_ERROR,
        COMMAND_NOT_FOUND,
        PARAM_NOT_FOUND,
        ERROR_JSON_FORMAT,
        //for connect - BEGIN
        LOGIN_AUTH_ERROR = 1000,
        //for connect - END
    }
}
