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
      { label: 'test', url: '/test' },
    ],
  },
  {
    label: 'Bãi đỗ xe',
    options: [
      {
        label: 'Danh sách bãi đỗ xe',
        url: routes.listParkingZone,
        title: 'Danh sách bãi gửi xe',
        desc: 'Toàn bộ danh sách bãi gửi xe hiển thị dưới đây',
      },
      {
        label: 'Danh sách yêu cầu',
        url: routes.viewRequestedParkingZones,
        title: 'Danh sách yêu cầu đăng ký bãi gửi xe',
        desc: 'Toàn bộ danh sách yêu cầu bãi gửi xe hiển thị dưới đây',
      },
    ],
  },
];

export const ownerSidebar = [
  {
    label: 'User',
    options: [
      {
        label: 'Profile',
        url: '/profile',
        title: 'Thông tin tài khoản',
        desc: 'Mọi thông tin về tài khoản được hiển thị dưới đây',
      },
      { label: 'test', url: '/test' },
    ],
  },
  {
    label: 'Bãi đỗ xe',
    options: [
      {
        label: 'Danh sách bãi đỗ xe',
        url: routes.listParkingZone,
        title: 'Danh sách bãi gửi xe',
        desc: 'Toàn bộ danh sách bãi gửi xe hiển thị dưới đây',
      },
      {
        label: 'Đăng ký bãi đỗ xe',
        url: routes.registerParkingZone,
        title: 'Form đăng ký bãi gửi xe',
        desc: 'Điền form dưới đây để đăng ký bãi gửi xe mới',
      },
    ],
  },
  {
    label: 'Nhân viên',
    options: [
      {
        label: 'Danh sách nhân viên',
        url: routes.listAttendant,
        title: 'Danh sách nhân viên',
        desc: 'Toàn bộ danh sách nhân viên hiển thị dưới đây',
      },
    ],
  },
];
