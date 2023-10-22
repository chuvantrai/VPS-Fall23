/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable no-unused-vars */
import { useEffect, useState } from 'react';
import { Table, Divider, Button, Image, Pagination, notification } from 'antd';
import Swal from 'sweetalert2';

import useParkingZoneService from '@/services/parkingZoneService';

function ViewRequestedParkingZones() {
  const columns = [
    {
      title: 'Tên',
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Địa chỉ',
      dataIndex: 'detailAddress',
      key: 'detailAddress',
    },
    {
      title: 'Slots',
      dataIndex: 'slots',
      key: 'slots',
    },
    {
      title: 'Thời gian đăng ký',
      dataIndex: 'createdAt',
      key: 'createdAt',
    },
    {
      title: 'Action',
      dataIndex: '',
      key: 'x',
      render: (_, record) => (
        <>
          <Button
            type="primary"
            className="bg-[#1677ff]"
            onClick={(e) => {
              e.preventDefault();
              handleApprove(record.id);
            }}
          >
            Approve
          </Button>
          <Divider type="vertical" />
          <Button
            type="primary"
            danger
            onClick={(e) => {
              e.preventDefault();
              handleReject(record.id);
            }}
          >
            Reject
          </Button>
        </>
      ),
    },
  ];

  const service = useParkingZoneService();
  const [dataSrc, setDataSrc] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalItems, setTotalItems] = useState();

  const getRequestParkingZones = (currentPage) => {
    service
      .getRequestParkingZones({ pageNumber: currentPage })
      .then((res) => {
        console.log(res);
        setDataSrc(res?.data.data);
        setTotalItems(res?.data.totalCount);
      })
      .catch((err) => {
        notification.error({
          message: err?.message,
        });
      });
  };

  const handleApprove = (parkingZoneId) => {
    Swal.fire({
      title: 'Bạn có chắc chắn muốn chấp nhận?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Có',
      cancelButtonText: 'Hủy',
      showLoaderOnConfirm: true,
      preConfirm: () => {
        const params = {
          id: parkingZoneId,
          isApprove: true,
        };

        service.changeParkingZoneStat(params).catch((err) => {
          Swal.showValidationMessage(`Request failed: ${err}`);
        });
      },
    }).then((result) => {
      if (result.isConfirmed) {
        Swal.fire({
          title: 'Thành Công!',
          icon: 'success',
          allowOutsideClick: false,
        }).then((result) => {
          if (result.isConfirmed) {
            getRequestParkingZones(currentPage);
          }
        });
      }
    });
  };

  const handleReject = (parkingZoneId) => {
    Swal.fire({
      title: 'Bạn có chắc chắn muốn từ chối?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Có',
      cancelButtonText: 'Hủy',
    }).then(async (result) => {
      if (result.isConfirmed) {
        const { value: text } = await Swal.fire({
          input: 'textarea',
          inputLabel: 'Lý do từ chối',
          inputPlaceholder: 'Nhập lý do vào đây...',
          inputAttributes: {
            'aria-label': 'Type your message here',
          },
        });

        if (text) {
          const params = {
            id: parkingZoneId,
            isApprove: false,
            rejectReason: text,
          };

          service
            .changeParkingZoneStat(params)
            .then((res) => {
              Swal.fire({
                title: 'Thành công!',
                icon: 'success',
              }).then((result) => {
                if (result.isConfirmed) {
                  getRequestParkingZones(currentPage);
                }
              });
            })
            .catch((err) => {
              Swal.fire({
                title: 'Có lỗi xảy ra!',
                icon: 'error',
              });
            });
        }
      }
    });
  };

  const handleChangePage = (page) => {
    setCurrentPage(page);
    getRequestParkingZones(page);
  };

  useEffect(() => {
    getRequestParkingZones();
  }, []);

  return (
    <div className="d-flex w-full h-full">
      <Table
        className="w-full h-full"
        columns={columns}
        expandable={{
          expandedRowRender: (record) => (
            <div className="d-flex flex-col">
              <Image.PreviewGroup>
                {record.parkingZoneImages.map((img, index) => (
                  <div key={index} className="mr-[10px] inline-block">
                    <Image className="rounded-[6px]" width={200} src={img} />
                  </div>
                ))}
              </Image.PreviewGroup>
              <p>
                Giá Tiền (mỗi tiếng): <span className="font-medium">{record.pricePerHour}</span>
              </p>
              <p>
                Giá Tiền Quá Giờ (mỗi tiếng): <span className="font-medium">{record.priceOverTimePerHour}</span>
              </p>
            </div>
          ),
          rowExpandable: (record) => record.name !== 'Not Expandable',
        }}
        dataSource={dataSrc}
        pagination={false}
      />
      <div className="py-[16px] flex flex-row-reverse pr-[24px]">
        <Pagination current={currentPage} onChange={handleChangePage} total={totalItems} showSizeChanger={false} />
      </div>
    </div>
  );
}

export default ViewRequestedParkingZones;
