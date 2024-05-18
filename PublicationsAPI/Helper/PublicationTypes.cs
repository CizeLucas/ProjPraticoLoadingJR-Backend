using System;

namespace PublicationsAPI.Helper
{
    public static class PublicationTypes
    {
        public enum PublicationTypeEnum : byte
        {
            unknown = 0,
            Blog = 1,
            Edict = 2,
            News = 3,
            Advertising = 4,
            Event = 5,
            Tutorial = 6,
            Research = 7,
        }

        public static int getIntValueFromString(string PublicationStringType)
        {
            if (Enum.TryParse(PublicationStringType, true, out PublicationTypeEnum publicationType))
            {
                return (int)publicationType;
            }
            return (int)PublicationTypeEnum.unknown; // Default case if string value not found
        }

        public static string getStringValueFromInt(byte PublicationIntType)
        {
            if (Enum.IsDefined(typeof(PublicationTypeEnum), PublicationIntType))
            {
                return Enum.GetName(typeof(PublicationTypeEnum), PublicationIntType);
            }
            return PublicationTypeEnum.unknown.ToString(); // Default case if int value not found
        }
    }
}

