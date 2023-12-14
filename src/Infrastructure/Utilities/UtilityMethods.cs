namespace BetterSteamBrowser.Infrastructure.Utilities;

public class UtilityMethods
{
    public static readonly Dictionary<string, List<string>> CountryMap = new()
    {
        ["Europe"] =
        [
            "AL", "AD", "AM", "AT", "AZ", "BY", "BE", "BA", "BG", "HR", "CY", "CZ", "DK",
            "EE", "FO", "FI", "FR", "GE", "DE", "GI", "GR", "HU", "IS", "IE", "IT", "KZ",
            "XK", "LV", "LI", "LT", "LU", "MT", "MC", "ME", "NL", "MK", "NO", "PL", "PT",
            "MD", "RO", "RU", "SM", "RS", "SK", "SI", "ES", "SJ", "SE", "CH", "TR", "UA",
            "GB", "VA"
        ],
        ["Americas"] =
        [
            "AG", "BS", "BB", "BZ", "CA", "CR", "CU", "DM", "DO", "SV", "GL", "GD", "GT",
            "HT", "HN", "JM", "MX", "NI", "PA", "PR", "KN", "LC", "VC", "TT", "US", "AR",
            "BO", "BR", "CL", "CO", "EC", "FK", "GF", "GY", "PY", "PE", "SR", "UY", "VE"
        ],
        ["Asia"] =
        [
            "AF", "AM", "AZ", "BH", "BD", "BT", "BN", "KH", "CN", "CY", "GE", "IN", "ID",
            "IR", "IQ", "IL", "JP", "JO", "KZ", "KP", "KR", "KW", "KG", "LA", "LB", "MO",
            "MY", "MV", "MN", "MM", "NP", "OM", "PK", "PS", "PH", "QA", "SA", "SG", "LK",
            "SY", "TW", "TJ", "TH", "TL", "TM", "AE", "UZ", "VN", "YE"
        ],
        ["Africa"] =
        [
            "DZ", "AO", "BJ", "BW", "BF", "BI", "CV", "CM", "CF", "TD", "KM", "CG", "CD",
            "DJ", "EG", "GQ", "ER", "SZ", "ET", "GA", "GM", "GH", "GN", "GW", "CI", "KE",
            "LS", "LR", "LY", "MG", "MW", "ML", "MR", "MU", "YT", "MA", "MZ", "NA", "NE",
            "NG", "RE", "RW", "SH", "ST", "SN", "SC", "SL", "SO", "ZA", "SS", "SD", "TZ",
            "TG", "TN", "UG", "EH", "ZM", "ZW"
        ],
        ["Oceania"] =
        [
            "AS", "AU", "CK", "FJ", "PF", "GU", "KI", "MH", "FM", "NR", "NC", "NZ", "NU",
            "NF", "MP", "PW", "PG", "PN", "WS", "SB", "TK", "TO", "TV", "UM", "VU", "WF"
        ]
    };
}
