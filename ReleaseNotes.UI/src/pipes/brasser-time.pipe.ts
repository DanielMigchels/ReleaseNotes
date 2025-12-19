import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'brasserTime'
})
export class BrasserTimePipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    if (!value) return 'Onbekend';

    let dateStr: string;

    if (value instanceof Date) {
      dateStr = value.toISOString();
    } else {
      dateStr = value.toString();
    }

    const [year, month, day] = dateStr.split('T')[0].split('-');

    return `${day}-${month}-${year}`;
  }

}
