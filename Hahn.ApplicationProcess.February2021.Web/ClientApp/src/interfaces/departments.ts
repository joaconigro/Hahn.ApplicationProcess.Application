export interface IDepartment {
  id: number;
  name: string;
}

export function departments(): IDepartment[] {
  return [
    { id: 0, name: 'HQ'},
    { id: 0, name: 'Store 1'},
    { id: 0, name: 'Store 2'},
    { id: 0, name: 'Store 3'},
    { id: 0, name: 'Maintenance Station' },
  ];
}
