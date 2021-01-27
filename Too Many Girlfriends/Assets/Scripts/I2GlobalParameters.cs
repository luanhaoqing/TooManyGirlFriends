namespace I2.Loc
{

    public class I2GlobalParameters : RegisterGlobalParameters
    {
        public override string GetParameterValue(string ParamName)
        {
            if (ParamName == "PLAYER_NAME")
                return this.GetComponent<GameMaster>().GetCurrentSpeakerName();
            return null;
        }
    }
}