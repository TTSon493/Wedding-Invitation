import { COUNTRIES } from "../lib/countries";

export type SelectMenuOption = typeof COUNTRIES[number]

export interface Country {
    title: string;
    value: string;
}