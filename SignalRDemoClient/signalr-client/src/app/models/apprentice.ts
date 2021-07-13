import { ComprehensionLevel } from "./comprehension-level";

export interface Apprentice {
    name: string,
    id: number,
    contentComprehension: ComprehensionLevel
}