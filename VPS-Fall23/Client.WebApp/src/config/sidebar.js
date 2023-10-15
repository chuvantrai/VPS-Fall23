import routes from '@/config/routes';

export const adminSidebar = [
  {
    label: 'User',
    options: [
      { label: 'Profile', url: '/profile' },
      { label: 'test', url: '/' },
    ],
  },
  {
    label: 'Manage',
    options: [
      { label: 'View Parking Zone List', url: '/listParkingZone' },
      { label: 'Register Parking Zone', url: routes.registerParkingZone },
      { label: 'Requested Parking Zone', url: routes.viewRequestedParkingZones },
    ],
  },
];
