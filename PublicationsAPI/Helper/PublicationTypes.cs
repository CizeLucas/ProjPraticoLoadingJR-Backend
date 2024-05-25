using System;

namespace PublicationsAPI.Helper
{
    public static class PublicationTypes
    {
        public enum PublicationTypeEnum : int
        {
            UNKNOWN = 0,
            BLOG = 1,
            EDICT = 2,
            NEWS = 3,
            ADVERTISING = 4,
            EVENT = 5,
            TUTORIAL = 6,
            RESEARCH = 7,
        }

        public static int getIntValueFromString(string PublicationStringType)
        {
            if (Enum.TryParse(PublicationStringType, true, out PublicationTypeEnum publicationType))
            {
                return (int)publicationType;
            }
            return (int)PublicationTypeEnum.UNKNOWN; // Default case if string value not found
        }

        public static string getStringValueFromInt(int PublicationIntType)
        {
            if (Enum.IsDefined(typeof(PublicationTypeEnum), PublicationIntType))
            {
                return Enum.GetName(typeof(PublicationTypeEnum), PublicationIntType);
            }
            return PublicationTypeEnum.UNKNOWN.ToString(); // Default case if int value not found
        }
    }
}

