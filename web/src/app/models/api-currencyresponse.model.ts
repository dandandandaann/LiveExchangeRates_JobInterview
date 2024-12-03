export interface ApiCurrencyResponse {
    timestamp: number;
    currencyPair: string;
    rates: Record<string, number>;
}