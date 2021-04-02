import { I18NService } from "./i18n-service";

export function stringUtcToDate(value): Date | any {
  const isoDateFormat = /^(\d{4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2})$/;

  if (isoDateFormat.test(value)) {
    const values = (value as string).split(/ |-|:/);
    const date = new Date();
    date.setUTCFullYear(parseInt(values[0]), parseInt(values[1]) - 1, parseInt(values[2]));
    date.setUTCHours(parseInt(values[3]), parseInt(values[4]));
    return date;
  }
  return value;
}

export function dateToUtcString(value): string | any {
  const isoDateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?(Z)?$/;

  if (isoDateFormat.test(value)) {
    const date = new Date(value);
    return `${date.getUTCFullYear()}-${pad(date.getUTCMonth() + 1, 2)}-${pad(date.getUTCDate(), 2)} ${pad(date.getUTCHours(), 2)}:${pad(date.getUTCMinutes(), 2)}`;
  }
  return value;
}

export function pad(num: number, size: number): string {
  let result = num.toString();
  while (result.length < size) result = '0' + result;
  return result;
}

export function mapDepartment(value: number, i18n: I18NService): string {
  return i18n.tr(`Department${value}`);
}
