import { InjectionToken } from '@angular/core';

export const BASE_PATH = new InjectionToken<string>('http://localhost:60191');
export const COLLECTION_FORMATS = {
    'csv': ',',
    'tsv': '   ',
    'ssv': ' ',
    'pipes': '|'
}
