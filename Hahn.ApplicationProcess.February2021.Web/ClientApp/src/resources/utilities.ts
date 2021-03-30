export function stringUtcToDate(value): Date | any {
  const isoDateFormat = /^(\d{4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2})$/;

  if (isoDateFormat.test(value)) {
    const values = (value as string).split(' ');
    const date = new Date(values[0]);
    const time = values[1].split(':');
    date.setUTCHours(parseInt(time[0]), parseInt(time[1]));
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
