export interface ApiCurrencyResponse {
    timestamp: number;
    exchangePair: string;
    rates: Record<string, number>;
}