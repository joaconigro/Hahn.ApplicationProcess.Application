define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.mapDepartment = exports.pad = exports.dateToUtcString = exports.stringUtcToDate = void 0;
    function stringUtcToDate(value) {
        const isoDateFormat = /^(\d{4}-\d{1,2}-\d{1,2} \d{1,2}:\d{1,2})$/;
        if (isoDateFormat.test(value)) {
            const values = value.split(/ |-|:/);
            const date = new Date();
            date.setUTCFullYear(parseInt(values[0]), parseInt(values[1]) - 1, parseInt(values[2]));
            date.setUTCHours(parseInt(values[3]), parseInt(values[4]));
            return date;
        }
        return value;
    }
    exports.stringUtcToDate = stringUtcToDate;
    function dateToUtcString(value) {
        const isoDateFormat = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?(Z)?$/;
        if (isoDateFormat.test(value)) {
            const date = new Date(value);
            return `${date.getUTCFullYear()}-${pad(date.getUTCMonth() + 1, 2)}-${pad(date.getUTCDate(), 2)} ${pad(date.getUTCHours(), 2)}:${pad(date.getUTCMinutes(), 2)}`;
        }
        return value;
    }
    exports.dateToUtcString = dateToUtcString;
    function pad(num, size) {
        let result = num.toString();
        while (result.length < size)
            result = '0' + result;
        return result;
    }
    exports.pad = pad;
    function mapDepartment(value, i18n) {
        return i18n.tr(`Department${value}`);
    }
    exports.mapDepartment = mapDepartment;
});
//# sourceMappingURL=utilities.js.map