/* eslint-disable react-hooks/exhaustive-deps */
import { Table, Button, Pagination } from 'antd';
import { useEffect, useState } from 'react';

import useFeedbackServices from '@/services/feedbackServices';
import { getAccountJwtModel } from '@/helpers';
import ReplyModal from './components/ReplyModal';

function ListFeedback() {
  const service = useFeedbackServices();
  const account = getAccountJwtModel();

  const columns = [
    {
      title: 'Bãi đỗ xe',
      dataIndex: 'parkingZoneName',
      key: 'parkingZoneName',
      render: (text) => <a>{text}</a>,
    },
    {
      title: 'Người tạo',
      dataIndex: 'email',
      key: 'email',
    },
    {
      title: 'Thời gian tạo',
      dataIndex: 'createdAt',
      key: 'createdAt',
    },
    {
      title: 'Đánh giá',
      key: 'rate',
      dataIndex: 'rate',
    },
    {
      title: 'Nội dung đánh giá',
      key: 'content',
      dataIndex: 'content',
      width: '20%',
    },
    {
      title: 'Phản hồi',
      key: 'replies',
      dataIndex: 'replies',
      width: '20%',
      render: (_, { replies }) => replies.map((reply) => reply.content),
    },
    {
      title: 'Action',
      key: 'action',
      render: (_, record) => (
        <>
          {record.replies.length == 0 && (
            <Button
              type="primary"
              onClick={() => {
                handleOpenReplyModal(record.id);
              }}
            >
              Trả lời bình luận
            </Button>
          )}
          {record.replies.length > 0 && <></>}
        </>
      ),
    },
  ];

  const [data, setData] = useState([]);
  const [replyModalOpen, setReplyModalOpen] = useState(false);
  const [feedbackId, setFeedbackId] = useState('');
  const [pageNumber, setPageNumber] = useState(1);
  const [totalItems, setTotalItems] = useState(0);

  const handleOpenReplyModal = (feedbackId) => {
    setFeedbackId(feedbackId);
    setReplyModalOpen(true);
  };

  const handleChangePage = (page) => {
    setPageNumber(page);
  };

  const loadData = () => {
    service.getFeedbackForOwner(account.UserId, pageNumber).then((res) => {
      setData(res.data.data);
      setTotalItems(res.data.totalCount);
    });
  };

  useEffect(() => {
    loadData();
  }, [pageNumber]);

  const handleReplyFeedback = (values) => {
    service.addReplyToFeedback(values);
    setReplyModalOpen(false);
    loadData();
  };

  return (
    <div className="w-[100%]">
      <Table columns={columns} dataSource={data} pagination={false} />
      <div className="py-[16px] flex flex-row-reverse pr-[24px]">
        <Pagination current={pageNumber} onChange={handleChangePage} total={totalItems} showSizeChanger={false} />
      </div>
      <ReplyModal
        open={replyModalOpen}
        feedbackId={feedbackId}
        onCreate={handleReplyFeedback}
        onCancel={() => {
          setReplyModalOpen(false);
        }}
      />
    </div>
  );
}

export default ListFeedback;
