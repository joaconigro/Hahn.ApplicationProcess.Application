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
    return `${date.getUTCFullYear()}-${date.getUTCMonth() + 1}-${date.getUTCDate()} ${date.getUTCHours()}:${date.getUTCMinutes()}`;
  }
  return value;
}

export function mapDepartment(value: number, i18n: I18NService): string {
  return i18n.tr(`Department${value}`);
}
