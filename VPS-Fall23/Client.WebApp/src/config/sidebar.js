import routes from '@/config/routes';

export const adminSidebar = [
  {
    label: 'User',
    options: [
      {
        label: 'Profile',
        url: '/profile',
        title: 'Thông tin tài khoản',
        desc: 'Mọi thông tin về tài khoản được hiển thị dưới đây',
      },
      { label: 'test', url: '/' },
    ],
  },
  {
    label: 'Manage',
    options: [
      {
        label: 'View Parking Zone List',
        url: '/listParkingZone',
        title: 'Danh sách bãi gửi xe',
        desc: 'Toàn bộ danh sách bãi gửi xe hiển thị dưới đây',
      },
      { label: 'Register Parking Zone', url: routes.registerParkingZone },
      { label: 'Requested Parking Zone', url: routes.viewRequestedParkingZones },
    ],
  },
];
